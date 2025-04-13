// Presentation/Controllers/ResumeController.cs
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Infrastructure.Persistence;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResumeController : ControllerBase
    {
        private readonly IFileTextExtractor _fileTextExtractor;
        private readonly IResumeParser _parser;
        private readonly IResumeRepository _resumeRepository;

        public ResumeController(
            IFileTextExtractor fileTextExtractor,
            IResumeParser parser,
            IResumeRepository resumeRepository)
        {
            _fileTextExtractor = fileTextExtractor;
            _parser = parser;
            _resumeRepository = resumeRepository;
        }

        [HttpPost("parse-file")]
        public async Task<ActionResult<ResumeDto>> ParseFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string fileText;
            using (var stream = file.OpenReadStream())
            {
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

            var parsedResume = await _parser.ParseAsync(fileText);
            await _resumeRepository.AddAsync(parsedResume);

            var dto = new ResumeDto
            {
                Id = parsedResume.Id,
                Name = parsedResume.Name,
                Email = parsedResume.Email,
                Skills = parsedResume.Skills,
                Experience = parsedResume.Experience.Select(e => new ExperienceItemDto
                {
                    Position = e.Position,
                    Company = e.Company,
                    Duration = e.Duration,
                    Responsibilities = e.Responsibilities
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<List<ResumeDto>>> GetAll()
        {
            var resumes = await _resumeRepository.GetAllAsync();

            var dtoList = resumes.Select(r => new ResumeDto
            {
                Id = r.Id,
                Name = r.Name,
                Email = r.Email,
                Skills = r.Skills,
                Experience = r.Experience.Select(e => new ExperienceItemDto
                {
                    Position = e.Position,
                    Company = e.Company,
                    Duration = e.Duration,
                    Responsibilities = e.Responsibilities
                }).ToList()
            }).ToList();

            return Ok(dtoList);
        }
    }
}
