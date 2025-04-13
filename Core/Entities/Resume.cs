// Core/Entities/Resume.cs
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Resume
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<string> Skills { get; set; }

        public List<ExperienceItem> Experience { get; set; }
    }

    public class ExperienceItem
    {
        [Key]
        public int Id { get; set; }

        public string Position { get; set; }
        public string Company { get; set; }
        public string Duration { get; set; }

        public List<string> Responsibilities { get; set; }
    }
}
