using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.CategoryDtos;
using ModelLayer.Dtos.SpecialDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Mapping.AutoMapper.Profiles
{
    public class SpecialProfile:Profile
    {
        public SpecialProfile()
        {
            CreateMap<Special, SpecialGetDto>();

            CreateMap<SpecialPostDto, Special>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src =>
                    src.Picture != null ? ConvertToByteArray(src.Picture) : null));

            CreateMap<SpecialPutDto, Special>()
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

