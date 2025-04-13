// Presentation/Controllers/ResumeController.cs
using Core.Interfaces;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

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

            return Ok(MapToDto(parsedResume));
        }

        [HttpGet]
        public async Task<ActionResult<List<ResumeDto>>> GetAll()
        {
            var resumes = await _resumeRepository.GetAllAsync();
            return Ok(resumes.Select(MapToDto).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResumeDto>> GetById(int id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null) return NotFound();

            return Ok(MapToDto(resume));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ResumeDto updatedDto)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null) return NotFound();

            resume.Name = updatedDto.Name;
            resume.Email = updatedDto.Email;
            resume.Skills = updatedDto.Skills ?? new List<string>();
            resume.Experience = (updatedDto.Experience ?? new List<ExperienceItemDto>()).Select(e => new ExperienceItem
            {
                Position = e.Position,
                Company = e.Company,
                Duration = e.Duration,
                Responsibilities = e.Responsibilities
            }).ToList();

            await _resumeRepository.UpdateAsync(resume);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null) return NotFound();

            await _resumeRepository.DeleteAsync(resume);
            return NoContent();
        }

        private ResumeDto MapToDto(Resume resume)
        {
            return new ResumeDto
            {
                Id = resume.Id,
                Name = resume.Name,
                Email = resume.Email,
                Skills = resume.Skills,
                Experience = resume.Experience?.Select(e => new ExperienceItemDto
                {
                    Position = e.Position,
                    Company = e.Company,
                    Duration = e.Duration,
                    Responsibilities = e.Responsibilities
                }).ToList()
            };
        }
    }
}
