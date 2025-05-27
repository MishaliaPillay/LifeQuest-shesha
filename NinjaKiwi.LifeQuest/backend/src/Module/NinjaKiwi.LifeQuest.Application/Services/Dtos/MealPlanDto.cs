using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using NinjaKiwi.LifeQuest.Domain.Domain;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    [AutoMap(typeof(MealPlan))]
    public class MealPlanDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public MealPlanStatus Status { get; set; }
        public List<MealDto> Meals { get; set; }
    }

}
