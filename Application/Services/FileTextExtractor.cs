using Core.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Xceed.Words.NET;

namespace Application.Services;

public class FileTextExtractor : IFileTextExtractor
{
    public async Task<string> ExtractPdfAsync(Stream stream)
    {
        using var pdfReader = new PdfReader(stream);
        using var pdfDoc = new PdfDocument(pdfReader);
        string text = "";

        for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
        {
            text += PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i));
        }

        return text;
    }

    public async Task<string> ExtractDocxAsync(Stream stream)
    {
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        using var doc = DocX.Load(ms);
        return doc.Text;
    }
}
