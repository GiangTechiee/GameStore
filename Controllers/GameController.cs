using GameStore.Data;
using GameStore.Models.DTOs.Game;
using GameStore.Models.Entities;
using GameStore.Repositories.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class GameController : Controller
    {
        private readonly IGameRepository _repository;
        private readonly GameStoreContext _context;

        public GameController(IGameRepository repository, GameStoreContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet("game-admin")]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGames()
        {
            var games = await _repository.GetAllAsync();
            var gameDtos = games.Select(game => new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            }).ToList();
            return Ok(gameDtos);
        }

        [HttpGet("game-admin/{id}")]
        public async Task<ActionResult<GameDto>> GetGameForAdmin(int id)
        {
            var game = await _repository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var gameDto = new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            };
            return Ok(gameDto);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesForAdmin()
        {
            var games = await _repository.GetAllAsync();
            var gameDtos = games.Select(game => new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            }).ToList();
            return Ok(gameDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<GameDto>> GetGame(int id)
        {
            var game = await _repository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            var gameDto = new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            };
            return Ok(gameDto);
        }

        [HttpPost]
        public async Task<ActionResult<GameDto>> CreateGame([FromBody] CreateGameDto createGameDto)
        {
            var game = new Game
            {
                Title = createGameDto.Title,
                Description = createGameDto.Description,
                Price = createGameDto.Price,
                ReleaseDate = createGameDto.ReleaseDate,
                Platform = createGameDto.Platform,
                Developers = createGameDto.Developers,
                Publishers = createGameDto.Publishers,
                Website = createGameDto.Website,
                IsGOTY = createGameDto.IsGOTY,
                GameCategories = createGameDto.CategoryIds?.Select(categoryId => new GameCategory
                {
                    CategoryId = categoryId
                }).ToList() ?? new List<GameCategory>(),
                GameImages = createGameDto.ImageUrls?.Select(url => new GameImage
                {
                    ImageUrl = url
                }).ToList() ?? new List<GameImage>()
            };
            await _repository.AddAsync(game);

            var savedGame = await _repository.GetByIdAsync(game.GameId);
            if (savedGame == null)
            {
                return StatusCode(500, "Failed to retrieve the created game.");
            }

            var gameDto = new GameDto
            {
                GameId = savedGame.GameId,
                Title = savedGame.Title,
                Description = savedGame.Description,
                Price = savedGame.Price,
                ReleaseDate = savedGame.ReleaseDate,
                Platform = savedGame.Platform,
                Developers = savedGame.Developers,
                Publishers = savedGame.Publishers,
                Website = savedGame.Website,
                ViewCount = savedGame.ViewCount,
                IsGOTY = savedGame.IsGOTY,
                CategoryIds = savedGame.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = savedGame.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = savedGame.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            };
            return CreatedAtAction(nameof(GetGame), new { id = game.GameId }, gameDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, [FromBody] CreateGameDto updateGameDto)
        {
            var existingGame = await _context.Games
                .Include(g => g.GameCategories)
                .Include(g => g.GameImages)
                .FirstOrDefaultAsync(g => g.GameId == id);

            if (existingGame == null)
            {
                return NotFound();
            }

            if (updateGameDto.CategoryIds != null && updateGameDto.CategoryIds.Any())
            {
                var validCategoryIds = await _context.Categories
                    .Where(c => updateGameDto.CategoryIds.Contains(c.CategoryId))
                    .Select(c => c.CategoryId)
                    .ToListAsync();
                if (validCategoryIds.Count != updateGameDto.CategoryIds.Count)
                {
                    return BadRequest("One or more CategoryIds are invalid.");
                }
            }

            existingGame.Title = updateGameDto.Title;
            existingGame.Description = updateGameDto.Description;
            existingGame.Price = updateGameDto.Price;
            existingGame.ReleaseDate = updateGameDto.ReleaseDate;
            existingGame.Platform = updateGameDto.Platform;
            existingGame.Developers = updateGameDto.Developers;
            existingGame.Publishers = updateGameDto.Publishers;
            existingGame.Website = updateGameDto.Website;
            existingGame.IsGOTY = updateGameDto.IsGOTY;

            var currentCategoryIds = existingGame.GameCategories.Select(gc => gc.CategoryId).ToList();
            var newCategoryIds = updateGameDto.CategoryIds ?? new List<int>();

            var categoriesToRemove = existingGame.GameCategories
                .Where(gc => !newCategoryIds.Contains(gc.CategoryId))
                .ToList();
            foreach (var category in categoriesToRemove)
            {
                existingGame.GameCategories.Remove(category);
            }

            var categoriesToAdd = newCategoryIds
                .Where(cid => !currentCategoryIds.Contains(cid))
                .Select(cid => new GameCategory
                {
                    GameId = id,
                    CategoryId = cid
                })
                .ToList();
            foreach (var category in categoriesToAdd)
            {
                existingGame.GameCategories.Add(category);
            }

            await _repository.DeleteGameImagesAsync(id);

            if (updateGameDto.ImageUrls != null)
            {
                existingGame.GameImages = updateGameDto.ImageUrls.Select(url => new GameImage
                {
                    ImageUrl = url,
                    GameId = id
                }).ToList();
            }
            else
            {
                existingGame.GameImages = new List<GameImage>();
            }

            await _repository.UpdateAsync(existingGame);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _repository.GetByIdAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("platform/{platform}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesByPlatform(string platform)
        {
            var games = await _repository.GetByPlatformAsync(platform);
            var gameDtos = games.Select(game => new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            }).ToList();
            return Ok(gameDtos);
        }

        [HttpGet("sort/release-date")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesByReleaseDate()
        {
            var games = await _repository.GetByReleaseDateDescAsync();
            var gameDtos = games.Select(game => new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            }).ToList();
            return Ok(gameDtos);
        }

        [HttpGet("sort/view-count")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesByViewCount()
        {
            var games = await _repository.GetByViewCountDescAsync();
            var gameDtos = games.Select(game => new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            }).ToList();
            return Ok(gameDtos);
        }

        [HttpGet("goty/{isGOTY}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesByGOTY(bool isGOTY)
        {
            var games = await _repository.GetByGOTYAsync(isGOTY);
            var gameDtos = games.Select(game => new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            }).ToList();
            return Ok(gameDtos);
        }

        [HttpGet("category/{categoryId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GameDto>>> GetGamesByCategory(int categoryId)
        {
            var games = await _repository.GetByCategoryAsync(categoryId);
            var gameDtos = games.Select(game => new GameDto
            {
                GameId = game.GameId,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                ReleaseDate = game.ReleaseDate,
                Platform = game.Platform,
                Developers = game.Developers,
                Publishers = game.Publishers,
                Website = game.Website,
                ViewCount = game.ViewCount,
                IsGOTY = game.IsGOTY,
                CategoryIds = game.GameCategories?.Select(gc => gc.CategoryId).ToList() ?? new List<int>(),
                CategoryNames = game.GameCategories?.Select(gc => gc.Category.Name).ToList() ?? new List<string>(),
                GameImages = game.GameImages?.Select(img => new GameImageDto
                {
                    ImageId = img.ImageId,
                    ImageUrl = img.ImageUrl
                }).ToList() ?? new List<GameImageDto>()
            }).ToList();
            return Ok(gameDtos);
        }
    }
}