//  Presentation/Controllers/ResumeController.cs
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IFileTextExtractor _fileTextExtractor;
        private readonly IResumeParser _parser;

        public ResumeController(IFileTextExtractor fileTextExtractor, IResumeParser parser)
        {
            _fileTextExtractor = fileTextExtractor;
            _parser = parser;
        }

        [HttpPost("parse-file")]
        public async Task<ActionResult<Resume>> ParseFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string fileText;
            using (var stream = file.OpenReadStream())
            {
                // Extract plain text depending on file type
                if (file.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                {
                    fileText = await _fileTextExtractor.ExtractPdfAsync(stream);
                }
                else if (file.FileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase))
                {
                    fileText = await _fileTextExtractor.ExtractDocxAsync(stream);
                }
                else
                {
                    return BadRequest("Unsupported file type. Only .pdf and .docx are allowed.");
                }
            }

            var parsed = await _parser.ParseAsync(fileText);
            return Ok(parsed);
        }
    }
}
