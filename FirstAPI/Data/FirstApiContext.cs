using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace FirstAPI.Data
{
    public class FirstApiContext : DbContext
    {
        public FirstApiContext(DbContextOptions<FirstApiContext> options):base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>().HasData(
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
                IsDone = true
            },
            new TaskItem
            {
                Id = 3,
                Title = "Lunch",
                IsDone = false
            }

                );
        }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
