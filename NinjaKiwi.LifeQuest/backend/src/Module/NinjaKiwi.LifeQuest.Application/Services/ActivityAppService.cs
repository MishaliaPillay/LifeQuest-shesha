using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using NinjaKiwi.LifeQuest.Domain.Domain;

using Shesha.Services;
using Shesha.DynamicEntities.Dtos;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;
using Shesha;
using System.Linq;

namespace NinjaKiwi.LifeQuest.Services.Activities
{
    public class ActivityActionsAppService : SheshaAppServiceBase
    {
        private readonly IRepository<Activity, Guid> _activityRepository;
        private readonly IRepository<ActivityType, Guid> _activityTypeRepository;

        public ActivityActionsAppService(
            IRepository<Activity, Guid> activityRepository,
            IRepository<ActivityType, Guid> activityTypeRepository)
        {
            _activityRepository = activityRepository;
            _activityTypeRepository = activityTypeRepository;
        }

        [HttpPost, Route("api/app/activity-actions/create")]
        public async Task<ActivityDto> CreateActivityAsync([FromBody] CreateActivityDto input)
        {
            // Step 1: Create and insert Activity (without children)
            var activity = new Activity
            {
                Category = input.Category,
                Calories = input.Calories,
                Description = input.Description,
                Duration = input.Duration,
                Xp = input.Xp,
                Level = input.Level,
                IsComplete = input.IsComplete,
                Order = input.Order
            };

            await _activityRepository.InsertAsync(activity);
            await CurrentUnitOfWork.SaveChangesAsync(); // Ensures activity.Id is generated

            // Step 2: Add ActivityActivityType links now that Activity.Id exists
            activity.ActivityActivityTypes = new List<ActivityActivityType>();

            foreach (var typeId in input.ActivityTypeIds)
            {
                activity.ActivityActivityTypes.Add(new ActivityActivityType
                {
                    ActivityId = activity.Id,
                    ActivityTypeId = typeId
                });
            }

            await CurrentUnitOfWork.SaveChangesAsync(); // Persist the relationships

            // Step 3: Return DTO
            return new ActivityDto
            {
                Id = activity.Id,
                Category = activity.Category,
                Calories = activity.Calories,
                Description = activity.Description,
                Duration = activity.Duration,
                Xp = activity.Xp,
                Level = activity.Level,
                IsComplete = activity.IsComplete,
                Order = activity.Order,
                ActivityTypeIds = activity.ActivityActivityTypes.Select(x => x.ActivityTypeId).ToList()
            };
        }


    }
}
