namespace Application.Files;

public class TempFile : IDisposable
{
    public string FilePath { get; }
    
    private bool _disposed;

    public TempFile(string? extension = null)
    {
        FilePath = Path.GetTempFileName();

        if (!string.IsNullOrWhiteSpace(extension))
        {
            FilePath = Path.ChangeExtension(FilePath, extension);
        }

        File.Create(FilePath).Dispose();
    }

    public void WriteAllText(string content)
    {
        File.WriteAllText(FilePath, content);
    }

    public string ReadAllText()
    {
        return File.ReadAllText(FilePath);
    }

    public void WriteAllBytes(byte[] bytes)
    {
        File.WriteAllBytes(FilePath, bytes);
    }

    public byte[] ReadAllBytes()
    {
        return File.ReadAllBytes(FilePath);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }
        
        if (disposing)
        {
            // Managed resources are disposed here
        }

        // Unmanaged resources are disposed here
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Destructor
    ~TempFile()
    {
        Dispose(false);
    }
}