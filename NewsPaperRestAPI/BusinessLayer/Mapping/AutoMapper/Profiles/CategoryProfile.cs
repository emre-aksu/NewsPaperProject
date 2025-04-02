using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModelLayer.Dtos.AuthorDtos;
using ModelLayer.Dtos.CategoryDtos;
using ModelLayer.Entities;

namespace BusinessLayer.Mapping.AutoMapper.Profiles
{
   public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {
             CreateMap<Category, CategoryGetDto>();

            CreateMap<CategoryPostDto, Category>()
                .ForMember(dest => dest.Picture, opt => opt.MapFrom(src =>
                    src.Picture != null ? ConvertToByteArray(src.Picture) : null));

            CreateMap<CategoryPutDto, Category>()
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

