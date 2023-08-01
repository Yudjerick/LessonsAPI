using Microsoft.AspNetCore.Mvc;
using LessonsAPI.Models;
using LessonsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LessonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private LessonsAPIContext _context;

        public AuthController(LessonsAPIContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            return await _context.Authors.ToListAsync();
        }

        [HttpPost("reg")]
        public async Task<ActionResult<Author>> Register(string username, string password)
        {
            if(_context.Authors.Where(x=>x.Username == username).ToListAsync().Result.IsNullOrEmpty())
            {
                Author author = new Author(username, password);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return Ok(author);
            }
            return Problem($"username '{username}' already exists");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(long id)
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            var author = await _context.Authors.FindAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
