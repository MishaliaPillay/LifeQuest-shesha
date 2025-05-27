using AutoMapper;
using NinjaKiwi.LifeQuest.Domain.Domain;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;
using System.Linq;

using NinjaKiwi.LifeQuest.Domain;


namespace NinjaKiwi.LifeQuest.Common.Services.Mappings
{
    public class MealPlanMapProfile : Profile
    {
        public MealPlanMapProfile()
        {
            CreateMap<MealPlan, MealPlanDto>()
                    .ForMember(dest => dest.Meals, opt => opt.Ignore());

            CreateMap<MealPlanDay, MealPlanDayDto>()
                .ForMember(dest => dest.Meals, opt => opt.MapFrom(src => src.MealPlanDayMeals.Select(d => d.MealId)));
        }
    }
}
