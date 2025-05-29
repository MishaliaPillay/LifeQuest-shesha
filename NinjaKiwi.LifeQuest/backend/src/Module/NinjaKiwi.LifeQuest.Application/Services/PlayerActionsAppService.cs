using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using NinjaKiwi.LifeQuest.Domain.Domain;
using Shesha;
using Shesha.Services;
using Microsoft.Extensions.Logging;
using System.Linq;

using Abp.UI;
using NinjaKiwi.LifeQuest.Common.Services.Dtos;

namespace NinjaKiwi.LifeQuest.Services.Players
{
    public class PlayerActionsAppService : SheshaAppServiceBase
    {
        private readonly IRepository<Player, Guid> _playerRepository;
        private readonly ILogger<PlayerActionsAppService> _logger;

        public PlayerActionsAppService(IRepository<Player, Guid> playerRepository, ILogger<PlayerActionsAppService> logger)
        {
            _playerRepository = playerRepository;
            _logger = logger;
        }

        [HttpPut, Route("api/app/player-actions/update-description")]
        public async Task<PlayerDto> UpdateAvatarDescriptionAsync([FromBody] UpdateAvatarDescriptionDto input)
        {
            var player = await _playerRepository.GetAsync(input.PlayerId);
            if (player == null)
                throw new UserFriendlyException("Player not found.");

            player.AvatarDescription = input.Description;
            await _playerRepository.UpdateAsync(player);

            return MapToDto(player);
        }

        [HttpPost, Route("api/app/player-actions/generate-avatar")]
        public async Task<PlayerDto> GenerateAndSaveAvatarAsync([FromBody] GenerateAvatarDto input)
        {
            var player = await _playerRepository.GetAsync(input.PlayerId);
            if (player == null)
                throw new UserFriendlyException("Player not found.");

            var httpClient = new HttpClient();

            var defaultPrompt = "3D chibi style character, safe for work, high quality, colorful, big expressive eyes, small cute body, white background";
            var fullPrompt = string.IsNullOrWhiteSpace(player.AvatarDescription)
                ? defaultPrompt
                : $"{defaultPrompt}, {player.AvatarDescription}";

            var apiKey = "ELh4suii1gOEawyWXupJ61u039bioRAs0zs9aXCTxjXMuDaYkccACdrq5jIG";
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new UserFriendlyException("API key is not configured.");

            var requestBody = new
            {
                key = apiKey,
                prompt = fullPrompt,
                negative_prompt = "(worst quality:2), (low quality:2), blurry, watermark, text, nsfw",
                samples = "1",
                height = "1024",
                width = "1024",
                safety_checker = false
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync("https://modelslab.com/api/v6/realtime/text2img", httpContent);
                if (!response.IsSuccessStatusCode)
                    throw new UserFriendlyException($"Avatar generation failed: {response.StatusCode}");

                var responseString = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Avatar API response: {responseString}");

                string? imageUrl = ExtractImageUrl(responseString);
                if (string.IsNullOrEmpty(imageUrl))
                    throw new UserFriendlyException("Could not extract image URL.");

                player.Avatar = imageUrl;
                await _playerRepository.UpdateAsync(player);

                return MapToDto(player);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating avatar");
                throw new UserFriendlyException("Avatar generation failed.");
            }
        }

        [HttpGet, Route("api/app/player-actions/get/{id}")]
        public async Task<PlayerDto> GetPlayerByIdAsync(Guid id)
        {
            var player = await _playerRepository.GetAsync(id);
            return MapToDto(player);
        }

        private string? ExtractImageUrl(string response)
        {
            using var outerJson = JsonDocument.Parse(response);
            var root = outerJson.RootElement;

            if (root.TryGetProperty("output", out var outputArray) &&
                outputArray.ValueKind == JsonValueKind.Array &&
                outputArray.GetArrayLength() > 0)
            {
                return outputArray[0].GetString();
            }

            return null;
        }

        private PlayerDto MapToDto(Player player)
        {
            return new PlayerDto
            {
                Id = player.Id,
                UserId = player.User?.Id,
                Avatar = player.Avatar,
                AvatarDescription = player.AvatarDescription,
                Xp = player.Xp,
                Level = player.Level,

            };
        }
    }
}
