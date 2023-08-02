using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LessonsAPI.Data;
using LessonsAPI.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using LessonsAPI.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Diagnostics;
using LessonsAPI.RequestObjects;

namespace LessonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly LessonsAPIContext _context;

        public LessonsController(LessonsAPIContext context)
        {
            _context = context;
        }

        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonViewModel>>> GetLesson()
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            var viewModels = await _context.Lessons.Include(b => b.Tasks).Include(b => b.Author)
                .Select(x => LessonViewModel.ToViewModel(x)).ToListAsync();
            return viewModels;
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lesson>> GetLesson(long id)
        {
          if (_context.Lessons == null)
          {
              return NotFound();
          }
            var lesson = await _context.Lessons.FindAsync(id);
           
            if (lesson == null)
            {
                return NotFound();
            }
            _context.Entry(lesson).Collection(p => p.Tasks).Load();

            return lesson;
        }

        // PUT: api/Lessons/5
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutLesson(long id, LessonRequestObject requestObject)
        {
            var username = GetRequestHeaderUsername();
            var author = _context.Authors.FirstOrDefault(x => x.Username == username);
            if (author == null)
            {
                return BadRequest("Authorized person does not exist");
            }
            _context.Entry(author).Collection(x => x.Lesons).Load();

            if (!author.Lesons.Select(x => x.Id).Any(y => y == id))
            {
                return BadRequest("Try to modify lesson that does not belong to authorized person");
            }

            var modifiedLesson = LessonRequestObject.ToModel(requestObject, author, id);
            Lesson? lesson = _context.Lessons.FirstOrDefault(x => x.Id == id);
            if(lesson != null)
            {
                _context.Entry(lesson).Collection(x => x.Tasks).Load();
                lesson.Author = modifiedLesson.Author;
                lesson.Title = modifiedLesson.Title;
                lesson.Topic = modifiedLesson.Topic;
                lesson.Description = modifiedLesson.Description;

                _context.Tasks.RemoveRange(lesson.Tasks.ToArray());
                lesson.Tasks = modifiedLesson.Tasks;

                lesson.MofidiedAt = DateTime.UtcNow;
                _context.Entry(lesson).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LessonExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Ok(LessonViewModel.ToViewModel(lesson));
            }
            return NotFound();
           
        }

        // POST: api/Lessons
        [HttpPost, Authorize]
        public async Task<ActionResult<Lesson>> PostLesson(LessonRequestObject requestObject)
        {
            if (_context.Lessons == null)
            {
                return Problem("Entity set 'LessonsAPIContext.Lesson' is null.");
            }

            var username = GetRequestHeaderUsername();
            var author = _context.Authors.FirstOrDefault(x => x.Username == username);
            if (author == null)
            {
                return BadRequest("Authorized person does not exist");
            }

            Lesson lesson;
            lesson = LessonRequestObject.ToModel(requestObject, author, 0);

            lesson.CreatedAt = DateTime.UtcNow;
            lesson.MofidiedAt = DateTime.UtcNow;
            _context.Lessons.Add(lesson);
            await _context.SaveChangesAsync();

            return Ok(LessonViewModel.ToViewModel(lesson));
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteLesson(long id)
        {
            var username = GetRequestHeaderUsername();
            var author = _context.Authors.FirstOrDefault(x => x.Username == username);
            if (author == null)
            {
                return BadRequest("Authorized person does not exist");
            }

            _context.Entry(author).Collection(x => x.Lesons).Load();
            if (!author.Lesons.Select(x => x.Id).Any(y => y == id))
            {
                return BadRequest("Try to modify lesson that does not belong to authorized person");
            }
            var lesson = await _context.Lessons.FindAsync(id);
            if(lesson == null)
            {
                return BadRequest($"No lesson with id '{id}'");
            }
            
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LessonExists(long id)
        {
            return (_context.Lessons?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string GetRequestHeaderUsername()
        {
            Request.Headers.TryGetValue("Authorization", out var token);
            
            token = token.ToString().Split(' ')[1];
            Console.WriteLine(token);
            return new JwtSecurityTokenHandler().ReadJwtToken(token)
                .Claims.First(c => c.Type == ClaimTypes.Name).Value;
        }
    }
}
