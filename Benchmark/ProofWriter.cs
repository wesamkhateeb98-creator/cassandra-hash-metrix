using System.Diagnostics;
using System.Text;
using Anispay.EvidenceProofs;
using Cassandra;

internal static class ProofWriter
{
    internal static async Task<RunSummary> WriteAllAsync(ISession session, PreparedStatement insert, ProofRecord[] proofs, bool withHash)
    {
        var summary = new RunSummary(proofs.Length);

        await WriteAllAsync(session, insert, proofs, withHash, summary);

        return summary;
    }

    /// <summary>Writes every proof, adding to an existing <paramref name="summary"/> so several runs share one report.</summary>
    internal static async Task WriteAllAsync(ISession session, PreparedStatement insert, ProofRecord[] proofs, bool withHash, RunSummary summary)
    {
        foreach (var proof in proofs)
        {
            await WriteOneAsync(session, insert, proof, withHash, summary);
        }
    }

    /// <summary>One INSERT; adds its timings to <paramref name="summary"/> (Total and Gen0 accumulate per call).</summary>
    internal static async Task WriteOneAsync(ISession session, PreparedStatement insert, ProofRecord proof, bool withHash, RunSummary summary)
    {
        var gen0Before = GC.CollectionCount(0);

        var started = Stopwatch.GetTimestamp();

        // Allocation is measured on this thread around the synchronous part only: hash + bound values.
        var allocatedBefore = GC.GetAllocatedBytesForCurrentThread();

        var prepareStarted = Stopwatch.GetTimestamp();

        var hash = withHash ? ProofHash.Compute(proof) : null;

        var values = Values(proof, hash?.Hash);

        var prepareElapsed = Stopwatch.GetElapsedTime(prepareStarted);

        var allocated = GC.GetAllocatedBytesForCurrentThread() - allocatedBefore;

        var insertStarted = Stopwatch.GetTimestamp();

        await session.ExecuteAsync(insert.Bind(values));

        var insertElapsed = Stopwatch.GetElapsedTime(insertStarted);

        summary.Add(RowBytes(proof, hash?.Hash), allocated, hash?.Elapsed ?? TimeSpan.Zero, prepareElapsed, insertElapsed);

        summary.Total += Stopwatch.GetElapsedTime(started);

        summary.Gen0Collections += GC.CollectionCount(0) - gen0Before;
    }

    private static object[] Values(ProofRecord proof, byte[]? hash)
    {
        object[] tail =
        [
            new DateTimeOffset(proof.CapturedAtUtc), proof.ProofId, proof.CorrelationId, proof.DeviceId!,
            proof.Category.ToString(), proof.Request.Payload, proof.Request.Signature!, proof.Request.VerificationRef!,
            proof.Response.Payload, proof.Response.Signature!, proof.Response.VerificationRef!, proof.ResponseStatus,
            proof.HandshakeProofRef!,
        ];

        return hash is null ? [proof.SessionId, .. tail] : [proof.SessionId, hash, .. tail];
    }

    /// <summary>Bound column value bytes (UTF-8 text, blobs, 8 B timestamp, 4 B int); Cassandra's per-cell overhead is not included.</summary>
    private static long RowBytes(ProofRecord proof, byte[]? hash)
    {
        string[] texts =
        [
            proof.SessionId, proof.ProofId, proof.CorrelationId, proof.DeviceId!, proof.Category.ToString(),
            proof.Request.Payload, proof.Request.VerificationRef!, proof.Response.Payload, proof.Response.VerificationRef!,
            proof.HandshakeProofRef!,
        ];

        var textBytes = texts.Sum(text => (long)Encoding.UTF8.GetByteCount(text));

        var blobBytes = proof.Request.Signature!.Length + proof.Response.Signature!.Length + (hash?.Length ?? 0);

        return textBytes + blobBytes + sizeof(long) + sizeof(int);
    }
}
