using Microsoft.EntityFrameworkCore;
using Task_ = ToDoApp.Data.Entities.Task;

namespace ToDoApp.Data
{
    public class ToDoAppDbContext: DbContext
    {
        public ToDoAppDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Task_> Tasks { get; set; }
    }
}
