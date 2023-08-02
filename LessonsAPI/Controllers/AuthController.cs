using Microsoft.AspNetCore.Mvc;
using LessonsAPI.Models;
using LessonsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using LessonsAPI.RequestObjects;

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

        [HttpGet("isAuthorized"), Authorize]
        public async Task<ActionResult<string>> IsAuthorized()
        {
            return Ok("authorized");
        }



        [HttpPost("reg")]
        public async Task<ActionResult<Author>> Register(AuthorRequestObject requestObject)
        {
            if(_context.Authors.Where(x=>x.Username == requestObject.Username).ToListAsync().Result.IsNullOrEmpty())
            {
                Author author = new Author();
                author.Username = requestObject.Username;
                author.PasswordHash = BCrypt.Net.BCrypt.HashPassword(requestObject.Password);
                _context.Authors.Add(author);
                await _context.SaveChangesAsync();

                return Ok(author);
            }
            return BadRequest($"username '{requestObject.Username}' already exists");
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(AuthorRequestObject requestObject)
        {
            Author author;
            try
            {
                author = _context.Authors.First(x => x.Username == requestObject.Username);
            }
            catch (Exception ex)
            {
                return BadRequest($"User '{requestObject.Username}' not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(requestObject.Password, author.PasswordHash))
            {
                return BadRequest("Wrong password");
            }
            
            return Ok(CreateToken(author));
        }

        private string CreateToken(Author author)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, author.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yudjericksecretkey7845"));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(1),
                    signingCredentials: cred
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpDelete("{id}"), Authorize]
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
