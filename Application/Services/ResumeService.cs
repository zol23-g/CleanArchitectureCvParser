// Application/Services/ResumeService.cs
using Core.Entities;
using Core.Interfaces;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ResumeService
    {
        private readonly IResumeParser _parser;

        public ResumeService(IResumeParser parser)
        {
            _parser = parser;
        }

        public async Task<Resume> ExtractResumeAsync(byte[] file, string fileName)
        {
            // You might need to extract text from the file before this call
            var plainText = System.Text.Encoding.UTF8.GetString(file); // simple dummy logic
            return await _parser.ParseAsync(plainText);
        }
    }
}
