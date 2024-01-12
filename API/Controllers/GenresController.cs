using API.Data;
using API.Entities;
using API.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public GenresController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IEnumerable<Genre>> Get(int page = 1, int recordsToTake = 2)
        {
            return await _dbContext.Genres.AsNoTracking()
                .OrderBy(g => g.Name)
                .Paginate(page, recordsToTake)
                .ToListAsync();
        }

        [HttpGet("first")]
        public async Task<ActionResult<Genre>> GetFirst()
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name.Contains("z"));

            if (genre is null)
                return NotFound();

            return genre;
        }

        [HttpGet("filter")]
        public async Task<IEnumerable<Genre>> Filter(string name)
        {
            return await _dbContext.Genres.Where(g => g.Name.Contains(name)).ToListAsync();
        }
    }
}