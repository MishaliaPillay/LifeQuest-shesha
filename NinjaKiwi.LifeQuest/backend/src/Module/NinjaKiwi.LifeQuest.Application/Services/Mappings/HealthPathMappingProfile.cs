using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;
using NinjaKiwi.LifeQuest.Domain.Domain;

namespace NinjaKiwi.LifeQuest.Common.Services.Mappings
{
    public class HealthPathMappingProfile : Profile
    {
        public HealthPathMappingProfile()
        {
            CreateMap<CreateHealthPathDto, HealthPath>()
       .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))

       .ForMember(dest => dest.MealPlans, opt => opt.Ignore());



            CreateMap<UpdateHealthPathDto, HealthPath>()
                .ForMember(dest => dest.MealPlans, opt => opt.Ignore());


            CreateMap<HealthPath, HealthPathDto>();


            CreateMap<MealPlan, MealPlanDto>();
        }
    }

}
