using System.Buffers.Binary;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Anispay.EvidenceProofs;

public sealed record ProofHashResult(byte[] Hash, TimeSpan Elapsed);

/// <summary>SHA-256 over a canonical, length-prefixed encoding of every proof field (32 B result).</summary>
public static class ProofHash
{
    public static ProofHashResult Compute(ProofRecord proof)
    {
        var started = Stopwatch.GetTimestamp();

        using var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);

        AppendText(hasher, proof.ProofId);

        AppendText(hasher, proof.CorrelationId);

        AppendText(hasher, proof.Category.ToString());

        AppendText(hasher, proof.SessionId);

        AppendText(hasher, proof.DeviceId);

        AppendInt64(hasher, new DateTimeOffset(proof.CapturedAtUtc).ToUnixTimeMilliseconds());

        AppendMessage(hasher, proof.Request);

        AppendMessage(hasher, proof.Response);

        AppendInt64(hasher, proof.ResponseStatus);

        AppendText(hasher, proof.HandshakeProofRef);

        var hash = hasher.GetHashAndReset();

        return new(hash, Stopwatch.GetElapsedTime(started));
    }

    private static void AppendMessage(IncrementalHash hasher, ProofMessage message)
    {
        AppendText(hasher, message.Payload);

        AppendBytes(hasher, message.Signature);

        AppendText(hasher, message.VerificationRef);
    }

    private static void AppendText(IncrementalHash hasher, string? text) =>
        AppendBytes(hasher, text is null ? null : Encoding.UTF8.GetBytes(text));

    // Length prefix (-1 for null) keeps ("ab","c") and ("a","bc") from hashing the same.
    private static void AppendBytes(IncrementalHash hasher, byte[]? bytes)
    {
        AppendInt64(hasher, bytes?.Length ?? -1);

        if (bytes is not null)
        {
            hasher.AppendData(bytes);
        }
    }

    private static void AppendInt64(IncrementalHash hasher, long value)
    {
        Span<byte> buffer = stackalloc byte[sizeof(long)];

        BinaryPrimitives.WriteInt64BigEndian(buffer, value);

        hasher.AppendData(buffer);
    }
}
