using FirstAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        static private List<TaskItem> tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Id = 1,
                Title = "Breakfast at 8 Am",
                IsDone = true
            },
            new TaskItem
            {
                Id = 2,
                Title = "Submit the assignment",
                IsDone= true
            },
            new TaskItem
            {
                Id = 3,
                Title = "Lunch",
                IsDone = false
            }
        };
        [HttpGet]
        public ActionResult<List<TaskItem>> GetTasks()
        {
            return Ok(tasks);
        }
        [HttpGet("{id}")]
        public ActionResult<TaskItem> GetTaskItemById(int id)
        {
            var task = tasks.FirstOrDefault(x  =>  x.Id == id);
            if (task == null)
                return NotFound();

            return Ok(task);
        }

    }
}
