using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain
{
    [Table("LifQu_MealPlans")]
    [Entity(TypeShortAlias = "LifQu.MealPlan")]
    public class MealPlan : Entity<Guid>
    {
        public string Name { get; set; }
        public MealPlanStatus Status { get; set; }
        public Guid HealthPathId { get; set; }
        public virtual HealthPath HealthPath { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? CompletedAt { get; set; }

        public virtual ICollection<MealPlanDay> MealPlanDays { get; set; } = new List<MealPlanDay>();
        [ManyToMany("LifQu_MealPlanMeals", "MealPlanId", "MealId")]
        public virtual ICollection<Meal> Meals { get; set; } = new List<Meal>();

        public MealPlan()
        {
            CreationTime = DateTime.UtcNow;
        }
    }





}