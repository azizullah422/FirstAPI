using System.ComponentModel.DataAnnotations;

namespace FirstAPI.Models
{
    public class TaskItem
    {
        public int Id { get; set; }

        public String Title { get; set; }

        public bool IsDone { get; set; }

    }
}
