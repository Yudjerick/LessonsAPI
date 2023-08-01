using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace LessonsAPI.Models
{
    public class Task
    {
        public long Id { get; set; }
        public string? Phrase { get; set; }
        public List<string>? Answer { get; set; }

        public List<string>? AdditionalVariants { get; set; }

        [JsonIgnore]
        public Lesson? Lesson { get; set; }
    }
}
