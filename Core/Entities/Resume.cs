// Core/Entities/Resume.cs


using System.ComponentModel.DataAnnotations;


namespace Core.Entities
{
    public class Resume
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<string> Skills { get; set; }

        public List<ExperienceItem> Experience { get; set; }
    }

    public class ExperienceItem
    {
        
        public int Id { get; set; }

        public string Position { get; set; }
        public string Company { get; set; }
        public string Duration { get; set; }

        public List<string> Responsibilities { get; set; }
    }
}
