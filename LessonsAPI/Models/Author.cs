using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace LessonsAPI.Models
{
    public class Author
    {
        public long Id { get; set; }
        public string Username { get; set; }

        [JsonIgnore] 
        public string PasswordHash { get; set; }

        public List<Lesson> Lesons { get; set; }
    }
}
