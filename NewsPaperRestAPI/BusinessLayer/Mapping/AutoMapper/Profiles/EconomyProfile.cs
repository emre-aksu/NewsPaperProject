using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.CategoryDtos;
using ModelLayer.Dtos.EconomyDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Mapping.AutoMapper.Profiles
{
    public class EconomyProfile:Profile
    {
        public EconomyProfile()
        {
            CreateMap<Economy, EconomyGetDto>();

            CreateMap<EconomyPostDto, Economy>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src =>
                    src.Picture != null ? ConvertToByteArray(src.Picture) : null));

            CreateMap<EconomyPutDto, Economy>()
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
