using Cassandra;

// Same proofs, two runs:
//   run 1: proofs_by_session_no_hash  (PRIMARY KEY ((session_id), captured_at, proof_id), no hash computed)
//   run 2: proofs_by_session          (PRIMARY KEY ((session_id, proof_hash), captured_at, proof_id), SHA-256 computed)
// Usage: dotnet run -c Release -- [proofs=1000] [warmup=50] [host=127.0.0.1] [port=19042]
var proofCount = args.Length > 0 ? int.Parse(args[0]) : 1000;

var warmupCount = args.Length > 1 ? int.Parse(args[1]) : 50;

var host = args.Length > 2 ? args[2] : Environment.GetEnvironmentVariable("EVIDENCE_CASSANDRA_HOST") ?? "127.0.0.1";

var port = args.Length > 3 ? int.Parse(args[3]) : int.Parse(Environment.GetEnvironmentVariable("EVIDENCE_CASSANDRA_PORT") ?? "19042");

var steps = new StepReport();

using var cluster = Cluster.Builder().AddContactPoint(host).WithPort(port).Build();

using var session = await steps.RunAsync("Connect", "cluster.ConnectAsync", () => cluster.ConnectAsync("evidence_task2_local_rf1"));

await steps.RunAsync("CreateTablesAsync", "CREATE no-hash table + CREATE with-hash table", () => BenchmarkSteps.CreateTablesAsync(session));

var inserts = await steps.RunAsync("PrepareInsertsAsync", "prepare no-hash INSERT + prepare with-hash INSERT", () => BenchmarkSteps.PrepareInsertsAsync(session));

var (warmup, proofs) = steps.Run("CreateProofs", "create warmup proofs + create bench proofs", () => BenchmarkSteps.CreateProofs(warmupCount, proofCount));

await steps.RunAsync("WarmUpAsync", "write warmup no-hash + write warmup with-hash", () => BenchmarkSteps.WarmUpAsync(session, inserts, warmup));

var (withoutHash, withHash) = await steps.RunAsync("MeasureAsync", "write proofs no-hash + write proofs with-hash", () => BenchmarkSteps.MeasureAsync(session, inserts, proofs));

Console.WriteLine($"Cassandra {host}:{port}, session {proofs[0].SessionId}, proofs {proofCount}, warmup {warmupCount} (excluded).");

Console.WriteLine();

Console.WriteLine(RunSummary.Header);

Console.WriteLine(withoutHash.ToTableRow("1 no hash", "proofs_by_session_no_hash"));

Console.WriteLine(withHash.ToTableRow("2 with hash", "proofs_by_session"));

Console.WriteLine(RunSummary.ToDiffRow(withoutHash, withHash));

Console.WriteLine();

Console.WriteLine($"Hash cost per proof: +{withHash.AverageAllocatedBytes - withoutHash.AverageAllocatedBytes:0} B allocated, " +
    $"+{withHash.AverageHashMicroseconds:0.00} µs hashing, +{withHash.AverageRowBytes - withoutHash.AverageRowBytes:0} B stored.");

Console.WriteLine();

Console.WriteLine("Per-method report:");

Console.WriteLine(steps.ToTable());

Console.WriteLine();

Console.WriteLine($"Verify:SELECT proof_id FROM proofs_by_session_no_hash WHERE session_id = '{proofs[0].SessionId}';");
