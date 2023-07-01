using AutoMapper;
using ToDoApp.WebUI.Dto.TaskDto;
using Task_ = ToDoApp.Data.Entities.Task;
namespace ToDoApp.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Task_, TaskDetailsDto>().ReverseMap();
            CreateMap<Task_, PostTaskDto>().ReverseMap();
        }
    }
}