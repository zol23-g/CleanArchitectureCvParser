// Application/Services/ResumeService.cs
using Core.Entities;
using Core.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ResumeService
    {
        private readonly IResumeParser _parser;
        private readonly IFileTextExtractor _textExtractor;

        public ResumeService(IResumeParser parser, IFileTextExtractor textExtractor)
        {
            _parser = parser;
            _textExtractor = textExtractor;
        }

        public async Task<Resume> ExtractResumeAsync(byte[] fileBytes, string fileName)
        {
            using var stream = new MemoryStream(fileBytes);
            string plainText;

            if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
            {
                plainText = await _textExtractor.ExtractPdfAsync(stream);
            }
            else if (fileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
            {
                plainText = await _textExtractor.ExtractDocxAsync(stream);
            }
            else
            {
                throw new NotSupportedException("Unsupported file type. Only .pdf and .docx are supported.");
            }

            return await _parser.ParseAsync(plainText);
        }
    }
}
