using System;
using Abp.AutoMapper;
using NinjaKiwi.LifeQuest.Domain.Domain;

namespace NinjaKiwi.LifeQuest.Common.Services.Dtos
{
    [AutoMap(typeof(HealthPath))]
    public class CreateHealthPathDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid PlayerId { get; set; }

    }
}
