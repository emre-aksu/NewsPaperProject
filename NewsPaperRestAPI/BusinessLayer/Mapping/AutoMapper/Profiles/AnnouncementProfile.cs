using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.AnnouncementDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Mapping.AutoMapper.Profiles
{
    public class AnnouncementProfile : Profile
    {
        public AnnouncementProfile()
        {

            CreateMap<Announcement, AnnouncementGetDto>();

            CreateMap<AnnouncementPostDto, Announcement>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src =>
                    src.Picture != null ? ConvertToByteArray(src.Picture) : null));

            CreateMap<AnnouncementPutDto, Announcement>()
                 .ForMember(dest => dest.Picture, opt => opt.MapFrom(src =>
                     src.Picture != null ? ConvertToByteArray(src.Picture) : null));
        }


        private byte[] ConvertToByteArray(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
