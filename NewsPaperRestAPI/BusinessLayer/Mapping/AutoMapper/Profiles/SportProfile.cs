using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.CategoryDtos;
using ModelLayer.Dtos.SportDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Mapping.AutoMapper.Profiles
{
    public class SportProfile:Profile
    {
        public SportProfile()
        {
            CreateMap<Sport, SportGetDto>();

            CreateMap<SportPostDto, Sport>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src =>
                    src.Picture != null ? ConvertToByteArray(src.Picture) : null));

            CreateMap<SportPutDto, Sport>()
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

