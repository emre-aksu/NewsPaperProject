using BusinessLayer.Contracts;
using BusinessLayer.Implementation;
using BusinessLayer.Mapping.AutoMapper.Profiles;
using BusinessLayer.ValidationRules.FluentValidation;
using DataAccessLayer.Contracts.IRepositories;
using DataAccessLayer.EntityFrameWork.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ModelLayer.Dtos.AgendaDtos;
using ModelLayer.Dtos.AnnouncementDtos;
using ModelLayer.Dtos.AuthorDtos;
using ModelLayer.Dtos.CategoryDtos;
using ModelLayer.Dtos.EconomyDtos;
using ModelLayer.Dtos.HomeDtos;
using ModelLayer.Dtos.SpecialDtos;
using ModelLayer.Dtos.SportDtos;

namespace BusinessLayer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //------------------Profile--------------------------

            services.AddAutoMapper(typeof(AgendaProfile));
            services.AddAutoMapper(typeof(AnnouncementProfile));
            services.AddAutoMapper(typeof(AuthorProfile));
            services.AddAutoMapper(typeof(CategoryProfile));
            services.AddAutoMapper(typeof(EconomyProfile));
            services.AddAutoMapper(typeof(HomeProfile));
            services.AddAutoMapper(typeof(SpecialProfile));
            services.AddAutoMapper(typeof(SportProfile));

            //-------------------Manager-------------------------
            services.AddScoped<IAgendaManager, AgendaManager>();
            services.AddScoped<IAnnouncementManager, AnnouncementManager>();
            services.AddScoped<IAuthorManager, AuthorManager>();
            services.AddScoped<ICategoryManager, CategoryManager>();
            services.AddScoped<IEconomyManager, EconomyManager>();
            services.AddScoped<IHomeManager, HomeManager>();
            services.AddScoped<ISpecialManager, SpecialManager>();
            services.AddScoped<ISportManager, SportManager>();

        


            //-----------------Repository Registrations----------
            services.AddScoped<IAgendaRepository, AgendaRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IEconomyRepository, EconomyRepository>();
            services.AddScoped<IHomeRepository, HomeRepository>();
            services.AddScoped<ISpecialRepository, SpecialRepository>();
            services.AddScoped<ISportRepository, SportRepository>();

       
            //-----------------FluentValidation Registrations----------
            services.AddScoped<IValidator<AgendaPostDto>, AgendaPostDtoValidator>();
            services.AddScoped<IValidator<AgendaPutDto>, AgendaPutDtoValidator>();

            services.AddScoped<IValidator<AnnouncementPostDto>, AnnouncementPostDtoValidator>();
            services.AddScoped<IValidator<AnnouncementPutDto>, AnnouncementPutDtoValidator>();

            services.AddScoped<IValidator<AuthorPostDto>, AuthorPostDtoValidator>();
            services.AddScoped<IValidator<AuthorPutDto>, AuthorPutDtoValidator>();

            services.AddScoped<IValidator<CategoryPostDto>, CategoryPostDtoValidator>();
            services.AddScoped<IValidator<CategoryPutDto>, CategoryPutDtoValidator>();

            services.AddScoped<IValidator<EconomyPostDto>, EconomyPostDtoValidator>();
            services.AddScoped<IValidator<EconomyPutDto>, EconomyPutDtoValidator>();

            services.AddScoped<IValidator<HomePostDto>, HomePostDtoValidator>();
            services.AddScoped<IValidator<HomePutDto>, HomePutDtoValidator>();

            services.AddScoped<IValidator<SpecialPostDto>, SpecialPostDtoValidator>();
            services.AddScoped<IValidator<SpecialPutDto>, SpecialPutDtoValidator>();

            services.AddScoped<IValidator<SportPostDto>, SportPostDtoValidator>();
            services.AddScoped<IValidator<SportPutDto>, SportPutDtoValidator>();


        }
    }
}
