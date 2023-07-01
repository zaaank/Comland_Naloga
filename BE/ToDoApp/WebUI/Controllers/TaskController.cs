using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using ToDoApp.Data.Contracts;
using ToDoApp.WebUI.Dto.TaskDto;
using Task_ = ToDoApp.Data.Entities.Task;

namespace ToDoApp.WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public TaskController(ITaskRepository taskRepository, IMapper mapper)
        {
            this._taskRepository = taskRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// Get list of all available tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDetailsDto>>> GetTasks()
        {
            var tasks = await _taskRepository.GetAllAsync();
            var mapped = _mapper.Map<List<TaskDetailsDto>>(tasks);
            return Ok(mapped);
        }

        /// <summary>
        /// Get task by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDetailsDto>> GetTask(int id)
        {

            var task = await _taskRepository.GetAsync(id);

            if (task == null)
            {
                return NotFound("Task was not found");
            }

            var taskDetailsDto = _mapper.Map<TaskDetailsDto>(task);

            return Ok(taskDetailsDto);
        }

        /// <summary>
        /// Post a new task
        /// </summary>
        /// <param name="createTaskDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<object>> PostTask(PostTaskDto createTaskDto)
        {
            var task = _mapper.Map<Task_>(createTaskDto);
            var added = await _taskRepository.AddAsync(task);
            if (added == null)
            {
                return Problem("Entity set 'ToDoAppDbContext.Task'  is null.");
            }

            return CreatedAtAction("GetTask", new { id = added.Id }, added);
        }

        /// <summary>
        /// Override already existing task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateTaskDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(int id, TaskDetailsDto updateTaskDto)
        {
            if (id != updateTaskDto.Id)
            {
                return BadRequest("Ids doesn't match");
            }

            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                return NotFound("Task does not exist in database");
            }

            _mapper.Map(updateTaskDto, task);

            try
            {
                await _taskRepository.UpdateAsync(task);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TaskExists(id))
                {
                    return NotFound("Couldn't save the changes");
                }
                else
                {
                    return NotFound("There was an error with updating the task"); // do better
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Delete task by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                return NotFound("Task you are trying to delete does not exists");
            }
            await _taskRepository.DeleteAsync(id);
            return NoContent();
        }

        private async Task<bool> TaskExists(int id)
        {
            return await _taskRepository.Exists(id);
        }
    }
}
