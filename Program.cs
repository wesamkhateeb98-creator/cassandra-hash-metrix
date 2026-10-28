using Cassandra;

// Same proofs, two runs:
//   run 1: proofs_by_session_no_hash  (PRIMARY KEY ((session_id), captured_at, proof_id), no hash computed)
//   run 2: proofs_by_session          (PRIMARY KEY ((session_id, proof_hash), captured_at, proof_id), SHA-256 computed)
// Usage: dotnet run -c Release -- [proofs=1000] [warmup=50] [host=127.0.0.1] [port=19042]
var proofCount = args.Length > 0 ? int.Parse(args[0]) : 1000;

var warmupCount = args.Length > 1 ? int.Parse(args[1]) : 50;

var host = args.Length > 2 ? args[2] : Environment.GetEnvironmentVariable("EVIDENCE_CASSANDRA_HOST") ?? "127.0.0.1";

var port = args.Length > 3 ? int.Parse(args[3]) : int.Parse(Environment.GetEnvironmentVariable("EVIDENCE_CASSANDRA_PORT") ?? "19042");

using var cluster = Cluster.Builder().AddContactPoint(host).WithPort(port).Build();

using var session = await cluster.ConnectAsync("evidence_task2_local_rf1");

await session.ExecuteAsync(new SimpleStatement(BenchmarkTables.CreateNoHashTable));

await session.ExecuteAsync(new SimpleStatement(BenchmarkTables.CreateWithHashTable));

var insertNoHash = await session.PrepareAsync(BenchmarkTables.InsertNoHash);

var insertWithHash = await session.PrepareAsync(BenchmarkTables.InsertWithHash);

var run = Guid.NewGuid().ToString("N")[..8];

var warmup = BenchmarkProofs.Create("warmup-" + run, warmupCount);

var proofs = BenchmarkProofs.Create("bench-" + run, proofCount);

await ProofWriter.WriteAllAsync(session, insertNoHash, warmup, withHash: false);

await ProofWriter.WriteAllAsync(session, insertWithHash, warmup, withHash: true);

var withoutHash = await ProofWriter.WriteAllAsync(session, insertNoHash, proofs, withHash: false);

var withHash = await ProofWriter.WriteAllAsync(session, insertWithHash, proofs, withHash: true);

Console.WriteLine($"Cassandra {host}:{port}, session {proofs[0].SessionId}, proofs {proofCount}, warmup {warmupCount} (excluded).");

Console.WriteLine();

Console.WriteLine(RunSummary.Header);

Console.WriteLine(withoutHash.ToTableRow("1 no hash", "proofs_by_session_no_hash"));

Console.WriteLine(withHash.ToTableRow("2 with hash", "proofs_by_session"));

Console.WriteLine(RunSummary.ToDiffRow(withoutHash, withHash));

Console.WriteLine();

Console.WriteLine($"Hash cost per proof: +{withHash.AverageAllocatedBytes - withoutHash.AverageAllocatedBytes:0} B allocated, " +
    $"+{withHash.AverageHashMicroseconds:0.00} µs hashing, +{withHash.AverageRowBytes - withoutHash.AverageRowBytes:0} B stored.");

Console.WriteLine($"Verify: SELECT proof_id FROM proofs_by_session_no_hash WHERE session_id = '{proofs[0].SessionId}';");
