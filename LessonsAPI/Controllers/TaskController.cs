using LessonsAPI.Data;
using LessonsAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LessonsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private LessonsAPIContext _context;
        public TaskController(LessonsAPIContext context) {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetTasks()
        {
            if (_context.Lessons == null)
            {
                return NotFound();
            }
            var viewModels = await _context.Tasks.Select(x => TaskViewModel.ToViewModel(x)).ToListAsync();
            return viewModels;
        }
    }
}
