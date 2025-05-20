using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator.Infrastructure;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    public class CreateActivityTypeDto
    {
        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Calories must be greater than 0")]
        public int Calories { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string? Duration { get; set; }
    }
}
