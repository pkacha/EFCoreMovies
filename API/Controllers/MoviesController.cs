using API.Data;
using API.DTOs;
using API.Entities;
using API.Entities.Keyless;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public MoviesController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet("withCounts")]
        public async Task<ActionResult<IEnumerable<MovieWithCounts>>> GetWithCounts()
        {
            return await _dbContext.Set<MovieWithCounts>().ToListAsync();
        }

        [HttpGet("automapper/{id:int}")]
        public async Task<ActionResult<MovieDTO>> GetWithAutoMapper(int id)
        {
            var movieDTO = await _dbContext.Movies
                .ProjectTo<MovieDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movieDTO is null)
                return NotFound();

            movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(X => X.Id).ToList();

            return movieDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post(MovieCreationDTO movieCreationDTO)
        {
            var movie = _mapper.Map<Movie>(movieCreationDTO);

            movie.Genres.ForEach(g => _dbContext.Entry(g).State = EntityState.Unchanged);
            movie.CinemaHalls.ForEach(ch => _dbContext.Entry(ch).State = EntityState.Unchanged);

            if (movie.MoviesActors is not null)
            {
                var movieActorCount = movie.MoviesActors.Count;

                for (int i = 0; i < movieActorCount; i++)
                {
                    movie.MoviesActors[i].Order = i + 1;
                }
            }

            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

    }
}