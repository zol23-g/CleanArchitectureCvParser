//  Application/DTOs/ResumeDto.cs
namespace Application.DTOs
{
    public class ResumeDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public List<string>? Skills { get; set; }
        public List<ExperienceItemDto>? Experience { get; set; }
    }

    public class ExperienceItemDto
    {
        public required string Position { get; set; }
        public required string Company { get; set; }
        public required string Duration { get; set; }
        public required List<string> Responsibilities { get; set; }
    }
}
