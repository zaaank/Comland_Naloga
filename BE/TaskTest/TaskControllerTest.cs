using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoApp;
using ToDoApp.Data.Contracts;
using ToDoApp.WebUI.Controllers;
using ToDoApp.WebUI.Dto.TaskDto;
using Xunit;
using Task_ = ToDoApp.Data.Entities.Task;
namespace TaskTest
{
    public class TaskControllerTest
    {
        private readonly Mock<ITaskRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TaskController _controller;

        public TaskControllerTest()
        {
            _mockRepo = new Mock<ITaskRepository>();
            _mockMapper = new Mock<IMapper>();
            _controller = new TaskController(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task PostTask_WithValidDto_ReturnsCreatedResponse()
        {
            // Arrange
            var createTaskDto = new PostTaskDto { Naslov = "New Task" };
            var taskEntity = new Task_ { Id = 1, Naslov = "New Task" };
            _mockMapper.Setup(mapper => mapper.Map<Task_>(createTaskDto)).Returns(taskEntity);
            _mockRepo.Setup(repo => repo.AddAsync(taskEntity)).ReturnsAsync(taskEntity);

            // Act
            var result = await _controller.PostTask(createTaskDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal("GetTask", createdAtActionResult.ActionName);
            Assert.Equal(1, createdAtActionResult.RouteValues["id"]);
            Assert.Equal(taskEntity, createdAtActionResult.Value);
        }

        [Fact]
        public async Task PutTask_WithValidIdAndDto_ReturnsNoContent()
        {
            // Arrange
            var updateTaskDto = new TaskDetailsDto { Id = 1, Naslov = "Updated Task" };
            var existingTask = new Task_ { Id = 1, Naslov = "Existing Task" };
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync(existingTask);

            // Act
            var result = await _controller.PutTask(1, updateTaskDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockMapper.Verify(mapper => mapper.Map(updateTaskDto, existingTask), Times.Once);
            _mockRepo.Verify(repo => repo.UpdateAsync(existingTask), Times.Once);
        }

        [Fact]
        public async Task PutTask_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var updateTaskDto = new TaskDetailsDto { Id = 1, Naslov = "Updated Task" };
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync((Task_)null);

            // Act
            var result = await _controller.PutTask(1, updateTaskDto);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            _mockMapper.Verify(mapper => mapper.Map(updateTaskDto, It.IsAny<Task_>()), Times.Never);
            _mockRepo.Verify(repo => repo.UpdateAsync(It.IsAny<Task_>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTask_WithValidId_ReturnsNoContent()
        {
            // Arrange
            var existingTask = new Task_ { Id = 1, Naslov = "Existing Task" };
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync(existingTask);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mockRepo.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteTask_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetAsync(1)).ReturnsAsync((Task_)null);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
            _mockRepo.Verify(repo => repo.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}