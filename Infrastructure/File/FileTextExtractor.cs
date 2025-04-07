using Core.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Xceed.Words.NET;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.File
{
    public class FileTextExtractor : IFileTextExtractor
    {
        public async Task<string> ExtractPdfAsync(Stream stream)
        {
            using var reader = new PdfReader(stream);
            using var pdfDoc = new PdfDocument(reader);
            var text = new System.Text.StringBuilder();

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {
                text.Append(PdfTextExtractor.GetTextFromPage(pdfDoc.GetPage(i)));
            }

            return await Task.FromResult(text.ToString());
        }

        public async Task<string> ExtractDocxAsync(Stream stream)
        {
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            ms.Position = 0;

            using var doc = DocX.Load(ms);
            return doc.Text;
        }
    }
}
