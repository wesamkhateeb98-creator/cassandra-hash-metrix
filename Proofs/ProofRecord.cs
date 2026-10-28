namespace Anispay.EvidenceProofs;

// Stand-in for the real Anispay.EvidenceProofs types, shaped by how the benchmark uses them.
public enum ProofCategory
{
    Handshake,
    Business,
}

/// <summary>One side of a captured exchange: base64url payload, its signature, and the key reference that verifies it.</summary>
public sealed record ProofMessage(string Payload, byte[]? Signature, string? VerificationRef);

public sealed record ProofRecord(
    string ProofId,
    string CorrelationId,
    ProofCategory Category,
    string SessionId,
    string? DeviceId,
    DateTime CapturedAtUtc,
    ProofMessage Request,
    ProofMessage Response,
    int ResponseStatus,
    string? HandshakeProofRef);
