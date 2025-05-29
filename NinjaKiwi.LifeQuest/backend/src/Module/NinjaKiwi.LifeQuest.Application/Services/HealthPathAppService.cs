using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.Extensions.Logging;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;
using NinjaKiwi.LifeQuest.Domain.Domain;

namespace NinjaKiwi.LifeQuest.Common.Services
{
    public class HealthPathAppService : ApplicationService
    {
        private readonly IRepository<HealthPath, Guid> _healthPathRepo;
        private readonly IRepository<MealPlan, Guid> _mealPlanRepo;
        private readonly IRepository<WeightEntry, Guid> _weightEntryRepo;
        private readonly IRepository<Player, Guid> _playerRepo;
        private readonly ILogger<HealthPathAppService> _logger;

        public HealthPathAppService(
            IRepository<HealthPath, Guid> healthPathRepo,
            IRepository<MealPlan, Guid> mealPlanRepo,
            IRepository<WeightEntry, Guid> weightEntryRepo,
            IRepository<Player, Guid> playerRepo,
            ILogger<HealthPathAppService> logger)
        {
            _healthPathRepo = healthPathRepo;
            _mealPlanRepo = mealPlanRepo;
            _weightEntryRepo = weightEntryRepo;
            _playerRepo = playerRepo;
            _logger = logger;
        }
        public async Task<HealthPathDto> CreateAsync(CreateHealthPathDto input)
        {
            _logger.LogInformation("Creating HealthPath for Player ID: {PlayerId}", input.PlayerId);

            var healthPath = ObjectMapper.Map<HealthPath>(input);

            // IMPORTANT: set PathType explicitly
            healthPath.PathType = "Health";

            await _healthPathRepo.InsertAsync(healthPath);

            var player = await _playerRepo.GetAsync(input.PlayerId);
            player.SelectedPath = healthPath;

            await _playerRepo.UpdateAsync(player);

            var dto = ObjectMapper.Map<HealthPathDto>(healthPath);
            dto.PlayerId = input.PlayerId;

            return dto;
        }


        public async Task<HealthPathDto> GetByPlayerIdAsync(Guid playerId)
        {
            var player = await _playerRepo.GetAsync(playerId);

            if (player.SelectedPath == null)
                throw new UserFriendlyException("This player does not have a path assigned.");

            // Fetch the full HealthPath explicitly by ID
            var pathId = player.SelectedPath.Id;
            var healthPath = await _healthPathRepo.FirstOrDefaultAsync(p => p.Id == pathId);

            if (healthPath == null)
                throw new UserFriendlyException("The player's selected path is not a HealthPath.");

            var dto = ObjectMapper.Map<HealthPathDto>(healthPath);
            dto.PlayerId = playerId;

            return dto;
        }


        public async Task<HealthPathDto> GetAsync(Guid id)
        {
            var path = await _healthPathRepo.GetAsync(id);

            // 🔍 Look up the Player who selected this path
            var player = await _playerRepo.FirstOrDefaultAsync(p => p.SelectedPath.Id == id);

            var dto = ObjectMapper.Map<HealthPathDto>(path);
            dto.PlayerId = player?.Id ?? Guid.Empty; // Fallback to empty Guid

            return dto;
        }


        public async Task<List<HealthPathDto>> GetAllAsync()
        {
            var paths = await _healthPathRepo.GetAllListAsync();

            // Fetch all players that have a SelectedPath matching any of the returned HealthPaths
            var pathIds = paths.Select(p => p.Id).ToList();
            var players = await _playerRepo.GetAllListAsync(p => p.SelectedPath != null && pathIds.Contains(p.SelectedPath.Id));

            // Build a lookup from pathId to playerId
            var pathToPlayerMap = players
                .GroupBy(p => p.SelectedPath.Id)
                .ToDictionary(g => g.Key, g => g.First().Id);

            // Map HealthPaths to DTOs and include PlayerId from lookup
            var dtos = paths.Select(path =>
            {
                var dto = ObjectMapper.Map<HealthPathDto>(path);
                dto.PlayerId = pathToPlayerMap.TryGetValue(path.Id, out var playerId) ? playerId : Guid.Empty;
                return dto;
            }).ToList();

            return dtos;
        }
        public async Task<List<PlayerHealthPathDto>> GetAllPlayersWithHealthPathAsync()
        {
            // Get all HealthPaths
            var healthPaths = await _healthPathRepo.GetAllListAsync();
            var healthPathIds = healthPaths.Select(hp => hp.Id).ToList();

            // Get all Players with a SelectedPath that is a HealthPath
            var players = await _playerRepo.GetAllListAsync(p =>
                p.SelectedPath != null && healthPathIds.Contains(p.SelectedPath.Id)
            );

            // Only show valid players — example rule: must have FirstName
            var validPlayers = players
                .Where(p => !string.IsNullOrWhiteSpace(p.FirstName))
                .ToList();

            var result = validPlayers.Select(p =>
            {
                var healthPath = healthPaths.FirstOrDefault(hp => hp.Id == p.SelectedPath.Id);
                return new PlayerHealthPathDto
                {
                    PlayerId = p.Id,
                    FullName = $"{p.FirstName} {p.LastName}".Trim(),
                    HealthPathTitle = healthPath?.Title
                };
            }).ToList();

            return result;
        }


        public async Task<HealthPathDto> UpdateAsync(UpdateHealthPathDto input)
        {
            var path = await _healthPathRepo.GetAsync(input.Id);

            path.Title = input.Title;

            path.MealPlans.Clear();


            var newMealPlans = await _mealPlanRepo.GetAllListAsync(mp => input.MealPlanIds.Contains(mp.Id));


            foreach (var mp in newMealPlans) path.MealPlans.Add(mp);


            await _healthPathRepo.UpdateAsync(path);
            return ObjectMapper.Map<HealthPathDto>(path);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _healthPathRepo.DeleteAsync(id);
        }
    }
}
