using System.Text;
using System.Text.Json;
using Anispay.EvidenceProofs;

/// <summary>Business proofs shaped like captured traffic: base64url JSON payloads, 2 B request and response signatures.</summary>
internal static class BenchmarkProofs
{
    private static readonly DateTime Start = new(2044, 1, 1, 10, 0, 0, DateTimeKind.Utc);

    internal static ProofRecord[] Create(string prefix, int count)
    {
        var sessionId = prefix + "-session";

        return Enumerable.Range(0, count).Select(index => Create(sessionId, $"{prefix}-proof-{index:D6}", index)).ToArray();
    }

    private static ProofRecord Create(string sessionId, string proofId, int index)
    {
        var request = Encode(new { key_tag = "key1", nonce = "echo-" + index,
            letter = new { uri = "/pay", headers = new Dictionary<string, string> { ["X-Correlation-Id"] = proofId } } });

        var response = Encode(new { status = 200, nonce = "echo-" + index, body = "accepted" });

        return new(proofId, proofId, ProofCategory.Business, sessionId, "bench-device", Start.AddMilliseconds(index),
            new(request, [1, 2], "device:bench-device:K1:v1"),
            new(response, [7, 8], $"session:{sessionId}:intermediate:v1"), 200, sessionId + "-handshake");
    }

    private static string Encode(object value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)))
        .TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
