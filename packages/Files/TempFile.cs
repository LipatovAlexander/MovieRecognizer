namespace Files;

public sealed class TempFile : IDisposable
{
    public string FilePath { get; }
    public string FileName { get; }

    private bool _disposed;

    public TempFile(string? extension = null)
    {
        FilePath = Path.GetTempFileName();

        if (!string.IsNullOrWhiteSpace(extension))
        {
            FilePath = Path.ChangeExtension(FilePath, extension);
        }

        FileName = Path.GetFileName(FilePath);

        File.Create(FilePath).Dispose();
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }

        _disposed = true;
    }
}