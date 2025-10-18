namespace Domain.Common;

public static class TraceId
{
    private static readonly System.Threading.AsyncLocal<string?> _current = new System.Threading.AsyncLocal<string?>();

    public static string Current => _current.Value ?? System.Guid.NewGuid().ToString();

    public static void SetTraceId(string traceId)
    {
        if (!string.IsNullOrEmpty(traceId)) _current.Value = traceId;
    }

    public static void Clear() => _current.Value = null;
}
