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

        public Author() { }
        public Author(string username, string password)
        {
            Username = username;
            PasswordHash = GetHashString(password);
        }

        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }

        public bool CheckPassword(string password)
        {
            return PasswordHash == GetHashString(password);
        }
    }
}
