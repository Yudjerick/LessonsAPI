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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutLesson(long id, LessonViewModel lessonViewModel)
        {
            Lesson lesson;
            try
            {
                lesson = LessonViewModel.ToModel(lessonViewModel, _context);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            Request.Headers.TryGetValue("Authorization", out var token);
            token = token.ToString().Split(' ')[0];
            var username = GetRequestHeaderUsername();
            if (lesson.Author.Username != username)
            {
                return BadRequest("Try to modify lesson that does not belong to authorized person");
            }

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

            return NoContent();
        }

        // POST: api/Lessons
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost, Authorize]
        public async Task<ActionResult<Lesson>> PostLesson(LessonViewModel lessonViewModel)
        {
            if (_context.Lessons == null)
            {
                return Problem("Entity set 'LessonsAPIContext.Lesson' is null.");
            }

            Lesson lesson;
            try
            {
                lesson = LessonViewModel.ToModel(lessonViewModel, _context);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }

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
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            var lesson = await _context.Lessons.FindAsync(id);
            _context.Entry(lesson).Reference(x => x.Author).Load();
            if (lesson == null)
            {
                return NotFound();
            }
            var username = GetRequestHeaderUsername();
            if (lesson.Author.Username != username)
            {
                return BadRequest("Try to delete lesson that does not belong to authorized person");
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
