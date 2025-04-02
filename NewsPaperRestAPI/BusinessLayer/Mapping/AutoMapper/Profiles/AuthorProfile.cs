using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.AuthorDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Mapping.AutoMapper.Profiles
{
    public class AuthorProfile:Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorGetDto>();

            CreateMap<AuthorPostDto, Author>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src =>
                    src.Picture != null ? ConvertToByteArray(src.Picture) : null));

            CreateMap<AuthorPutDto, Author>()
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
