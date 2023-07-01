using Task_ = ToDoApp.Data.Entities.Task;

namespace ToDoApp.Data.Contracts
{
    public interface ITaskRepository : IGenericRepository<Task_>
    {
    }
}
