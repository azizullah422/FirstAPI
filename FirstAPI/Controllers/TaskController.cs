using Azure.Identity;
using FirstAPI.Data;
using FirstAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

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

        const string SMTP_HOST = "smtp.gmail.com";
        const int SMTP_PORT = 587;
        const string SMTP_USER = "m.azizullah000@gmail.com";
        const string SMTP_PASS = "iowywfiuilulhqev";

        public async Task SendEmailAsync(string to,string subject,string body)
        {
            to = to.Trim();
            if(String .IsNullOrWhiteSpace(to)|| !MailAddress.TryCreate(to, out var toAddr))
                throw new ArgumentException("Invalid" , nameof(to));
            var fromAddr = new MailAddress(SMTP_USER, "Task App");
            using var message = new MailMessage(fromAddr, toAddr)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            using var client = new SmtpClient(SMTP_HOST, SMTP_PORT)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(SMTP_USER, SMTP_PASS)
            };
            await client.SendMailAsync(message);
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

        public async Task<IActionResult> UpdateTaskItem(int id, TaskItem UpdatedTaskItem, string? to)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return NotFound();

            var wasDone = task.IsDone;
           
            task.Title = UpdatedTaskItem.Title;
            task.IsDone = UpdatedTaskItem.IsDone;
            await _context.SaveChangesAsync();

            if (!wasDone && task.IsDone && !string.IsNullOrWhiteSpace(to))
            {
                await SendEmailAsync(
                    to,
                    $"Task Completed :{task.Title}",
                    $"<p>{task.Title} (ID{task.Id}) was marked as done  at {DateTime.UtcNow:u}.</p>");

            }
            return Ok(new {task.Id,task.Title,task.IsDone});
                
            


        }

        [HttpPatch("{id}")]

        //public async Task<IActionResult> PatchTaskItem(int id, JsonPatchDocument<TaskItem> patchTaskItem)
        //{
        //    if (patchTaskItem == null)
        //    {
        //        return BadRequest();
        //    }

        //    var task = await _context.Tasks.FindAsync(id);
        //    if (task == null)
        //    {
        //        return NotFound();
        //    }
        //    patchTaskItem.ApplyTo(task);
            

        //    await _context.SaveChangesAsync();
        //    return NoContent();

        //}
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
