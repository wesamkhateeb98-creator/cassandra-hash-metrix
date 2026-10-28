internal static class BenchmarkTables
{
    internal const string CreateNoHashTable = """
        CREATE TABLE IF NOT EXISTS proofs_by_session_no_hash (
          session_id text, captured_at timestamp, proof_id text, correlation_id text,
          device_id text, operation_kind text, request_payload text, request_signature blob,
          request_verification_ref text, response_payload text, response_signature blob,
          response_verification_ref text, response_status_code int, handshake_proof_ref text,
          PRIMARY KEY ((session_id), captured_at, proof_id)
        ) WITH CLUSTERING ORDER BY (captured_at ASC, proof_id ASC)
        """;

    internal const string CreateWithHashTable = """
        CREATE TABLE IF NOT EXISTS proofs_by_session (
          session_id text, proof_hash blob, captured_at timestamp, proof_id text, correlation_id text,
          device_id text, operation_kind text, request_payload text, request_signature blob,
          request_verification_ref text, response_payload text, response_signature blob,
          response_verification_ref text, response_status_code int, handshake_proof_ref text,
          PRIMARY KEY ((session_id, proof_hash), captured_at, proof_id)
        ) WITH CLUSTERING ORDER BY (captured_at ASC, proof_id ASC)
        """;

    internal const string InsertNoHash = "INSERT INTO proofs_by_session_no_hash (session_id, captured_at, proof_id, correlation_id, device_id, operation_kind, request_payload, request_signature, request_verification_ref, response_payload, response_signature, response_verification_ref, response_status_code, handshake_proof_ref) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

    internal const string InsertWithHash = "INSERT INTO proofs_by_session (session_id, proof_hash, captured_at, proof_id, correlation_id, device_id, operation_kind, request_payload, request_signature, request_verification_ref, response_payload, response_signature, response_verification_ref, response_status_code, handshake_proof_ref) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
}
