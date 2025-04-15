// Presentation/Controllers/ResumeController.cs
using Core.Interfaces;
using Core.Entities;
using Core.Common;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs;

namespace Presentation.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
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
        public async Task<ActionResult<ApiResponse<ResumeDto>>> ParseFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(ApiResponse<ResumeDto>.Fail("No file uploaded."));

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
                    return BadRequest(ApiResponse<ResumeDto>.Fail("Unsupported file type. Only .pdf and .docx are allowed."));
                }
            }

            var parsedResume = await _parser.ParseAsync(fileText);
            await _resumeRepository.AddAsync(parsedResume);

            return Ok(ApiResponse<ResumeDto>.Ok(MapToDto(parsedResume)));
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<PagedResult<ResumeDto>>>> GetAllPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var pagedResult = await _resumeRepository.GetPagedAsync(pageNumber, pageSize);

            var dtoItems = pagedResult.Items.Select(MapToDto).ToList();

            var result = new PagedResult<ResumeDto>
            {
                Items = dtoItems,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };

            return Ok(ApiResponse<PagedResult<ResumeDto>>.Ok(result));
        }

        [HttpGet("cursor")]
        public async Task<ActionResult<ApiResponse<CursorPagedResult<ResumeDto>>>> GetByCursor(
            [FromQuery] int? lastSeenId = null,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? name = null,
            [FromQuery] string? email = null,
            [FromQuery] string? skill = null)
        {
            var result = await _resumeRepository.GetAfterCursorAsync(
                lastSeenId, pageSize, name, email);

            var dtoItems = result.Items.Select(MapToDto).ToList();

            var response = new CursorPagedResult<ResumeDto>
            {
                Items = dtoItems,
                PageSize = result.PageSize,
                LastSeenId = result.LastSeenId,
                NextCursor = result.NextCursor,
                HasMore = result.HasMore
            };

            return Ok(ApiResponse<CursorPagedResult<ResumeDto>>.Ok(response));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ResumeDto>>> GetById(int id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
                return NotFound(ApiResponse<ResumeDto>.Fail($"Resume with ID {id} not found."));

            return Ok(ApiResponse<ResumeDto>.Ok(MapToDto(resume)));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Update(int id, [FromBody] ResumeDto updatedDto)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
                return NotFound(ApiResponse<string>.Fail($"Resume with ID {id} not found."));

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

            return Ok(ApiResponse<string>.Ok("Resume updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var resume = await _resumeRepository.GetByIdAsync(id);
            if (resume == null)
                return NotFound(ApiResponse<string>.Fail($"Resume with ID {id} not found."));

            await _resumeRepository.DeleteAsync(resume);

            return Ok(ApiResponse<string>.Ok("Resume deleted successfully."));
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
