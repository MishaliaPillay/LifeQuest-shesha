using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shesha.Domain.Attributes;

namespace NinjaKiwi.LifeQuest.Domain.Domain.Enums
{
    /// <summary>
    /// Days used for organizing MealPlan via Kanban board
    /// </summary>
    [ReferenceList("Meals", "MealPlanDays")]
    public enum RefListMealPlanDays : long
    {
        [Description("Day 1")]
        Day1 = 1,

        [Description("Day 2")]
        Day2 = 2,

        [Description("Day 3")]
        Day3 = 3,

        [Description("Day 4")]
        Day4 = 4,

        [Description("Day 5")]
        Day5 = 5,

        [Description("Day 6")]
        Day6 = 6,

        [Description("Day 7")]
        Day7 = 7,

        [Description("Day 8")]
        Day8 = 8,

        [Description("Day 9")]
        Day9 = 9,

        [Description("Day 10")]
        Day10 = 10
    }
}
