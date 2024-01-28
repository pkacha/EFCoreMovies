using API.Data;
using API.DTOs;
using API.Entities;
using API.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GenresController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IEnumerable<Genre>> Get()
        {
            _dbContext.Logs.Add(new Log { Message = "Executing Get() from GenresController" });
            await _dbContext.SaveChangesAsync();

            return await _dbContext.Genres.AsNoTracking()
                //.OrderBy(g => g.Name)
                .OrderByDescending(g => EF.Property<DateTime>(g, "CreatedDate"))
                .ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Genre>> Get(int id)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
                return NotFound();

            var createDate = _dbContext.Entry(genre).Property<DateTime>("CreatedDate").CurrentValue;

            return Ok(new
            {
                Id = genre.Id,
                Name = genre.Name,
                CreatedDate = createDate
            });
        }


        [HttpPost]
        public async Task<ActionResult> Post(GenreCreationDTO genreCreationDTO)
        {
            var genreName = genreCreationDTO.Name;

            var genreExists = await _dbContext.Genres.AnyAsync(g => g.Name == genreName);

            if (genreExists)
                return BadRequest($"The genre with name {genreName} already exists.");

            var genre = _mapper.Map<Genre>(genreCreationDTO);
            _dbContext.Genres.Add(genre);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("several")]
        public async Task<ActionResult> Post(GenreCreationDTO[] genreCreationDTOs)
        {
            var genres = _mapper.Map<Genre[]>(genreCreationDTOs);

            _dbContext.Genres.AddRange(genres);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
                return NotFound();

            _dbContext.Remove(genre);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("softdelete/{id:int}")]
        public async Task<ActionResult> SoftDelete(int id)
        {
            var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
                return NotFound();

            genre.IsDeleted = true;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("restore/{id:int}")]
        public async Task<ActionResult> Restore(int id)
        {
            var genre = await _dbContext.Genres.IgnoreQueryFilters().FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
                return NotFound();

            genre.IsDeleted = false;
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}