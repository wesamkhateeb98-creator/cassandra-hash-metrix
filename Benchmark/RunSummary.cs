internal sealed class RunSummary(int count)
{
    internal const string Header =
        "| Run | Table | Proofs | Row B | Allocated B/proof | Hash µs | Prepare µs | Insert µs avg | Insert µs p50 | Insert µs p99 | Total ms | Proofs/s | Gen0 GCs |\n" +
        "|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|---:|";

    private readonly List<double> insertMicroseconds = new(count);

    private long rowBytes;

    private long allocatedBytes;

    private double hashMicroseconds;

    private double prepareMicroseconds;

    internal TimeSpan Total { get; set; }

    internal int Gen0Collections { get; set; }

    internal int Count => count;

    internal double AverageRowBytes => (double)rowBytes / count;

    internal double AverageAllocatedBytes => (double)allocatedBytes / count;

    internal double AverageHashMicroseconds => hashMicroseconds / count;

    internal double AveragePrepareMicroseconds => prepareMicroseconds / count;

    internal double AverageInsertMicroseconds => insertMicroseconds.Average();

    internal double ProofsPerSecond => count / Total.TotalSeconds;

    internal void Add(long row, long allocated, TimeSpan hash, TimeSpan prepare, TimeSpan insert)
    {
        rowBytes += row;

        allocatedBytes += allocated;

        hashMicroseconds += hash.TotalMicroseconds;

        prepareMicroseconds += prepare.TotalMicroseconds;

        insertMicroseconds.Add(insert.TotalMicroseconds);
    }

    /// <summary>Adds another run's samples, so per-iteration summaries roll up into one total.</summary>
    internal void Merge(RunSummary other)
    {
        rowBytes += other.rowBytes;

        allocatedBytes += other.allocatedBytes;

        hashMicroseconds += other.hashMicroseconds;

        prepareMicroseconds += other.prepareMicroseconds;

        insertMicroseconds.AddRange(other.insertMicroseconds);

        Total += other.Total;

        Gen0Collections += other.Gen0Collections;
    }

    internal double InsertPercentile(double percentile)
    {
        var sorted = insertMicroseconds.Order().ToArray();

        return sorted[Math.Min(sorted.Length - 1, (int)Math.Ceiling(percentile * sorted.Length) - 1)];
    }

    internal string ToTableRow(string run, string table) =>
        $"| {run} | {table} | {count} | {AverageRowBytes:0} | {AverageAllocatedBytes:0} | {AverageHashMicroseconds:0.00} | " +
        $"{AveragePrepareMicroseconds:0.00} | {AverageInsertMicroseconds:0} | {InsertPercentile(0.50):0} | {InsertPercentile(0.99):0} | " +
        $"{Total.TotalMilliseconds:0} | {ProofsPerSecond:0} | {Gen0Collections} |";

    /// <summary>Row "with hash − without hash"; positive = hashing costs more.</summary>
    internal static string ToDiffRow(RunSummary withoutHash, RunSummary withHash) =>
        $"| Δ (2 − 1) | | | {withHash.AverageRowBytes - withoutHash.AverageRowBytes:+0;-0;0} | " +
        $"{withHash.AverageAllocatedBytes - withoutHash.AverageAllocatedBytes:+0;-0;0} | " +
        $"{withHash.AverageHashMicroseconds - withoutHash.AverageHashMicroseconds:+0.00;-0.00;0} | " +
        $"{withHash.AveragePrepareMicroseconds - withoutHash.AveragePrepareMicroseconds:+0.00;-0.00;0} | " +
        $"{withHash.AverageInsertMicroseconds - withoutHash.AverageInsertMicroseconds:+0;-0;0} | " +
        $"{withHash.InsertPercentile(0.50) - withoutHash.InsertPercentile(0.50):+0;-0;0} | " +
        $"{withHash.InsertPercentile(0.99) - withoutHash.InsertPercentile(0.99):+0;-0;0} | " +
        $"{withHash.Total.TotalMilliseconds - withoutHash.Total.TotalMilliseconds:+0;-0;0} | " +
        $"{withHash.ProofsPerSecond - withoutHash.ProofsPerSecond:+0;-0;0} | " +
        $"{withHash.Gen0Collections - withoutHash.Gen0Collections:+0;-0;0} |";
}
