using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.CategoryDtos;
using ModelLayer.Dtos.HomeDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Mapping.AutoMapper.Profiles
{
    public class HomeProfile:Profile
    {
        public HomeProfile()
        {
            CreateMap<Home, HomeGetDto>();
            //CreateMap<HomePostDto, Home>();
            //CreateMap<HomePutDto, Home>();

            CreateMap<HomePostDto, Home>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src =>
                    src.Picture != null ? ConvertToByteArray(src.Picture) : null));

            CreateMap<HomePutDto, Home>()
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

