using apiChat.Models;
using AutoMapper;

namespace apiChat.DTOs
{
    public class AutoMapperPerfiles : Profile
    {
        public AutoMapperPerfiles()
        {
            CreateMap<ConexionUser, DtoConexionUser>().ReverseMap();
        }
    }
}
