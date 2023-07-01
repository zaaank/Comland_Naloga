using Task = ToDoApp.Data.Entities.Task;

using ToDoApp.Data.Contracts;

namespace ToDoApp.Data.Repositories
{
    /// <summary>
    /// Simple task repository for managing basic crud operations
    /// </summary>
    public class TaskRepository: GenericRepository<Task>, ITaskRepository
    {
        private readonly ToDoAppDbContext _context;

        public TaskRepository(ToDoAppDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
