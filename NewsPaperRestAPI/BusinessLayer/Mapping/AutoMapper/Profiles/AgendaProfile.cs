using AutoMapper;
using ModelLayer.Dtos.AgendaDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Mapping.AutoMapper.Profiles
{
    public class AgendaProfile:Profile
    {
        public AgendaProfile()
        {
            CreateMap<Agenda, AgendaGetDto>();
            CreateMap<AgendaPostDto, Agenda>();
            CreateMap<AgendaPutDto, Agenda>();
         
        }
    }
}
