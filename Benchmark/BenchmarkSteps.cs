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

    internal static (ProofRecord[] Warmup, ProofRecord[] Proofs) CreateProofs(int warmupCount, int proofCount)
    {
        var run = Guid.NewGuid().ToString("N")[..8];

        var warmup = BenchmarkProofs.Create("warmup-" + run, warmupCount);

        var proofs = BenchmarkProofs.Create("bench-" + run, proofCount);

        return (warmup, proofs);
    }

    internal static async Task WarmUpAsync(ISession session, (PreparedStatement NoHash, PreparedStatement WithHash) inserts, ProofRecord[] warmup)
    {
        await ProofWriter.WriteAllAsync(session, inserts.NoHash, warmup, withHash: false);

        await ProofWriter.WriteAllAsync(session, inserts.WithHash, warmup, withHash: true);
    }

    internal static async Task<(RunSummary WithoutHash, RunSummary WithHash)> MeasureAsync(
        ISession session, (PreparedStatement NoHash, PreparedStatement WithHash) inserts, ProofRecord[] proofs)
    {
        var withoutHash = await ProofWriter.WriteAllAsync(session, inserts.NoHash, proofs, withHash: false);

        var withHash = await ProofWriter.WriteAllAsync(session, inserts.WithHash, proofs, withHash: true);

        return (withoutHash, withHash);
    }
}
