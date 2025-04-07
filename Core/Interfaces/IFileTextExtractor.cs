namespace Core.Interfaces;

public interface IFileTextExtractor
{
    Task<string> ExtractPdfAsync(Stream stream);
    Task<string> ExtractDocxAsync(Stream stream);
}
