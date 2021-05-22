using AutoMapper;
using ToDoAPI.Model;

namespace ToDoAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ToDoItem, ToDoItemDTO>()
                .ReverseMap();
        }
    }
}
