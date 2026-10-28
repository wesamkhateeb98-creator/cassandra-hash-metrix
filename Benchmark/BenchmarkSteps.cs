using Anispay.EvidenceProofs;
using Cassandra;

/// <summary>The benchmark flow, one method per pair of operations (no-hash side, with-hash side).</summary>
internal static class BenchmarkSteps
{
    internal static async Task CreateTablesAsync(ISession session)
    {
        await session.ExecuteAsync(new SimpleStatement(BenchmarkTables.CreateNoHashTable));

        await session.ExecuteAsync(new SimpleStatement(BenchmarkTables.CreateWithHashTable));
    }

    internal static async Task<(PreparedStatement NoHash, PreparedStatement WithHash)> PrepareInsertsAsync(ISession session)
    {
        var noHash = await session.PrepareAsync(BenchmarkTables.InsertNoHash);

        var withHash = await session.PrepareAsync(BenchmarkTables.InsertWithHash);

        return (noHash, withHash);
    }

    internal static (ProofRecord[] Warmup, ProofRecord[][] Batches) CreateProofs(int warmupCount, int proofCount, int counter)
    {
        var run = Guid.NewGuid().ToString("N")[..8];

        var warmup = BenchmarkProofs.Create("warmup-" + run, warmupCount);

        // One fresh batch per iteration, so every iteration stores new rows instead of overwriting.
        var batches = Enumerable.Range(1, counter).Select(iteration => BenchmarkProofs.Create($"bench-{run}-i{iteration}", proofCount)).ToArray();

        return (warmup, batches);
    }

    internal static async Task WarmUpAsync(ISession session, (PreparedStatement NoHash, PreparedStatement WithHash) inserts, ProofRecord[] warmup)
    {
        await ProofWriter.WriteAllAsync(session, inserts.NoHash, warmup, withHash: false);

        await ProofWriter.WriteAllAsync(session, inserts.WithHash, warmup, withHash: true);
    }
}
