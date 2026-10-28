using System.Diagnostics;
using Cassandra;

// One loop, counter iterations; each iteration stores its own batch of proofs in both tables:
//   proofs_by_session_no_hash  (PRIMARY KEY ((session_id), captured_at, proof_id), no hash computed)
//   proofs_by_session          (PRIMARY KEY ((session_id, proof_hash), captured_at, proof_id), SHA-256 computed)
// Usage: dotnet run -c Release -- [proofs=1000] [warmup=50] [host=127.0.0.1] [port=19042] [counter=1]
var proofCount = args.Length > 0 ? int.Parse(args[0]) : 1000;

var warmupCount = args.Length > 1 ? int.Parse(args[1]) : 50;

var host = args.Length > 2 ? args[2] : Environment.GetEnvironmentVariable("EVIDENCE_CASSANDRA_HOST") ?? "127.0.0.1";

var port = args.Length > 3 ? int.Parse(args[3]) : int.Parse(Environment.GetEnvironmentVariable("EVIDENCE_CASSANDRA_PORT") ?? "19042");

var counter = args.Length > 4 ? int.Parse(args[4]) : 1;

Console.WriteLine($"PID {Environment.ProcessId}: starting in 5 seconds, {proofCount} proofs × counter {counter}...");

await Task.Delay(TimeSpan.FromSeconds(5));

var setup = new StepReport();

using var cluster = Cluster.Builder().AddContactPoint(host).WithPort(port).Build();

using var session = await setup.RunAsync("Connect", "cluster.ConnectAsync", () => cluster.ConnectAsync("evidence_task2_local_rf1"));

await setup.RunAsync("CreateTablesAsync", "CREATE no-hash table + CREATE with-hash table", () => BenchmarkSteps.CreateTablesAsync(session));

var inserts = await setup.RunAsync("PrepareInsertsAsync", "prepare no-hash INSERT + prepare with-hash INSERT", () => BenchmarkSteps.PrepareInsertsAsync(session));

var (warmup, batches) = setup.Run("CreateProofs", "create warmup proofs + create bench batches", () => BenchmarkSteps.CreateProofs(warmupCount, proofCount, counter));

await setup.RunAsync("WarmUpAsync", "write warmup no-hash + write warmup with-hash", () => BenchmarkSteps.WarmUpAsync(session, inserts, warmup));

var noHashSummary = new RunSummary(proofCount * counter);

var hashSummary = new RunSummary(proofCount * counter);

var loopStarted = Stopwatch.GetTimestamp();

for (var i = 0; i < counter; i++)
{
    // proofCount no-hash + proofCount with-hash inserts (1000 + 1000 = 2000 by default)
    var noHash = await ProofWriter.WriteAllAsync(session, inserts.NoHash, batches[i], withHash: false);

    var withHash = await ProofWriter.WriteAllAsync(session, inserts.WithHash, batches[i], withHash: true);

    Console.WriteLine();

    Console.WriteLine($"## Iteration {i + 1}/{counter}: {proofCount} + {proofCount} = {proofCount * 2} inserts");

    Console.WriteLine();

    Console.WriteLine(RunSummary.Header);

    Console.WriteLine(noHash.ToTableRow("1 no hash", "proofs_by_session_no_hash"));

    Console.WriteLine(withHash.ToTableRow("2 with hash", "proofs_by_session"));

    Console.WriteLine(RunSummary.ToDiffRow(noHash, withHash));

    noHashSummary.Merge(noHash);

    hashSummary.Merge(withHash);
}

var loopElapsed = Stopwatch.GetElapsedTime(loopStarted);

Console.WriteLine($"Cassandra {host}:{port}, {proofCount} proofs × counter {counter}, warmup {warmupCount} (excluded).");

Console.WriteLine();

Console.WriteLine("Setup per-method report:");

Console.WriteLine(setup.ToTable());

Console.WriteLine();

Console.WriteLine("## All iterations");

Console.WriteLine();

Console.WriteLine($"Loop: {counter} iteration(s) × {proofCount} proofs × 2 tables = {counter * proofCount * 2} inserts in {loopElapsed.TotalMilliseconds:0.00} ms.");

Console.WriteLine();

Console.WriteLine(RunSummary.Header);

Console.WriteLine(noHashSummary.ToTableRow("1 no hash", "proofs_by_session_no_hash"));

Console.WriteLine(hashSummary.ToTableRow("2 with hash", "proofs_by_session"));

Console.WriteLine(RunSummary.ToDiffRow(noHashSummary, hashSummary));

Console.WriteLine();

Console.WriteLine($"Hash cost per proof: +{hashSummary.AverageAllocatedBytes - noHashSummary.AverageAllocatedBytes:0} B allocated, " +
    $"+{hashSummary.AverageHashMicroseconds:0.00} µs hashing, +{hashSummary.AverageRowBytes - noHashSummary.AverageRowBytes:0} B stored.");

Console.WriteLine();

Console.WriteLine($"Verify: SELECT proof_id FROM proofs_by_session_no_hash WHERE session_id = '{batches[0][0].SessionId}';");
