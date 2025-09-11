using System.ComponentModel.DataAnnotations;

namespace FirstAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public String Title { get; set; } = String.Empty;

        public bool IsDone { get; set; }

    }
}
