using ToDoApp.Data.Contracts;
using Task = ToDoApp.Data.Entities.Task;

namespace ToDoApp.Bl
{
    public class TaskBusinessLogic
    {
        private readonly ITaskRepository _taskRepository;

        public TaskBusinessLogic(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        Task<List<Task>> GetAllAsync()
        {
            return _taskRepository.GetAllAsync();
        }
    }
}
