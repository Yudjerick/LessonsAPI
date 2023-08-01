using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace LessonsAPI.Models
{
    public class Lesson
    {
        public long Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }
       
        public string? Topic { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? MofidiedAt { get; set; }

        public Author Author { get; set; }

        public List<Task> Tasks { get; set; }
    }
}
