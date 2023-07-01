using Task = ToDoApp.Data.Entities.Task;

using ToDoApp.Data.Contracts;

namespace ToDoApp.Data.Repositories
{
    public class TaskRepository: GenericRepository<Task>, ITaskRepository
    {
        private readonly ToDoAppDbContext _context;

        public TaskRepository(ToDoAppDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
