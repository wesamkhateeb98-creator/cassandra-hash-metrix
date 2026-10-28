using System.Diagnostics;

/// <summary>Times each benchmark step and prints one table row per step.</summary>
internal sealed class StepReport
{
    internal const string Header =
        "| Step | Operations | Elapsed ms | Allocated B | Gen0 GCs |\n" +
        "|---|---|---:|---:|---:|";

    private readonly List<string> rows = [];

    private TimeSpan total;

    // Allocated B is process-wide (includes the driver's I/O threads), so async steps are counted fully.
    internal async Task<T> RunAsync<T>(string step, string operations, Func<Task<T>> action)
    {
        var gen0Before = GC.CollectionCount(0);

        var allocatedBefore = GC.GetTotalAllocatedBytes(precise: true);

        var started = Stopwatch.GetTimestamp();

        var result = await action();

        var elapsed = Stopwatch.GetElapsedTime(started);

        var allocated = GC.GetTotalAllocatedBytes(precise: true) - allocatedBefore;

        total += elapsed;

        rows.Add($"| {step} | {operations} | {elapsed.TotalMilliseconds:0.00} | {allocated} | {GC.CollectionCount(0) - gen0Before} |");

        return result;
    }

    internal Task RunAsync(string step, string operations, Func<Task> action) =>
        RunAsync(step, operations, async () =>
        {
            await action();

            return 0;
        });

    internal T Run<T>(string step, string operations, Func<T> action) =>
        RunAsync(step, operations, () => Task.FromResult(action())).GetAwaiter().GetResult();

    internal string ToTable() => string.Join('\n', [Header, .. rows, $"| **Total** | | {total.TotalMilliseconds:0.00} | | |"]);
}
