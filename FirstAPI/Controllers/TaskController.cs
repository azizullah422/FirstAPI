using FirstAPI.Data;
using FirstAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        //static private List<TaskItem> tasks = new List<TaskItem>
        //{
        //    new TaskItem
        //    {
        //        Id = 1,
        //        Title = "Breakfast at 8 Am",
        //        IsDone = true
        //    },
        //    new TaskItem
        //    {
        //        Id = 2,
        //        Title = "Submit the assignment",
        //        IsDone= true
        //    },
        //    new TaskItem
        //    {
        //        Id = 3,
        //        Title = "Lunch",
        //        IsDone = false
        //    }
        //};

        private readonly FirstApiContext _context;

        public TaskController(FirstApiContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<TaskItem>>> GetTask()
        {
            return Ok(await _context.Tasks.ToListAsync());
        }
        [HttpGet("offset")]
        public async Task<ActionResult<List<TaskItem>>> GetTasks(int limit = 5 , int page = 1)
        {
            var task = await _context.Tasks
                .AsNoTracking()
                .OrderBy(a => a.Id)
                .Skip(limit * (page - 1))
                .Take(limit)
                .ToListAsync();
            return Ok(task);
            
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskItemById(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]

        public async Task<ActionResult<TaskItem>> AddTaskItem(TaskItem newtask)
        {
            if (newtask == null)
                return BadRequest();

            _context.Tasks.Add(newtask);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTaskItemById), new { Id = newtask.Id }, newtask);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTaskItem(int id, TaskItem UpdatedTaskItem)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            task.Id = UpdatedTaskItem.Id;
            task.Title = UpdatedTaskItem.Title;
            task.IsDone = UpdatedTaskItem.IsDone;

            await _context.SaveChangesAsync();
            return NoContent();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskItem(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();

        }

    }
}
