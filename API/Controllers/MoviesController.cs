using API.Data;
using API.DTOs;
using API.Entities;
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {
            var movie = await _dbContext.Movies
                .Include(g => g.Genres.OrderByDescending(g => g.Name).Where(g => !g.Name.Contains("m")))
                .Include(c => c.CinemaHalls.OrderByDescending(ch => ch.Cinema.Name))
                .ThenInclude(ch => ch.Cinema)
                .Include(m => m.MovieActors)
                .ThenInclude(ma => ma.Actor)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
                return NotFound();

            var movieDTO = _mapper.Map<MovieDTO>(movie);
            movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(X => X.Id).ToList();

            return movieDTO;
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

        [HttpGet("selectloading/{id:int}")]
        public async Task<ActionResult> GetSelectLoading(int id)
        {
            var movieDTO = await _dbContext.Movies.Select(m => new
            {
                Id = m.Id,
                Title = m.Title,
                Genres = m.Genres.Select(g => g.Name).OrderByDescending(n => n).ToList()
            }).FirstOrDefaultAsync(m => m.Id == id);

            if (movieDTO is null)
                return NotFound();

            return Ok(movieDTO);
        }

        [HttpGet("explicitloading/{id:int}")]
        public async Task<ActionResult<MovieDTO>> GetExplicit(int id)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
                return NotFound();

            var genresCount = await _dbContext.Entry(movie).Collection(p => p.Genres).Query().CountAsync();

            var movieDTO = _mapper.Map<MovieDTO>(movie);

            return Ok(new
            {
                Id = movieDTO.Id,
                Title = movieDTO.Title,
                GenresCount = genresCount,
            });
        }

        [HttpGet("lazyloading/{id:int}")]
        public async Task<ActionResult<MovieDTO>> GetLazyLoading(int id)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null)
                return NotFound();

            var movieDTO = _mapper.Map<MovieDTO>(movie);

            movieDTO.Cinemas = movieDTO.Cinemas.DistinctBy(m => m.Id).ToList();

            return movieDTO;
        }

        [HttpGet("groupedByCinema")]
        public async Task<ActionResult> GetGroupedByCinemas()
        {
            var groupedMovies = await _dbContext.Movies.GroupBy(m => m.InCinemas).Select(g => new
            {
                InCinemas = g.Key,
                Count = g.Count(),
                Movies = g.ToList()
            }).ToListAsync();

            return Ok(groupedMovies);
        }

        [HttpGet("groupedByGenresCount")]
        public async Task<ActionResult> GetGroupedByGenresCount()
        {
            var groupedMovies = await _dbContext.Movies.GroupBy(g => g.Genres.Count()).Select(g => new
            {
                Count = g.Key,
                Titles = g.Select(m => m.Title),
                Genres = g.Select(m => m.Genres).SelectMany(a => a).Select(ge => ge.Name).Distinct()
            }).ToListAsync();

            return Ok(groupedMovies);
        }

        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> Filter([FromQuery] MovieFilterDTO movieFilterDTO)
        {
            var moviesQueryable = _dbContext.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(movieFilterDTO.Title))
                moviesQueryable = moviesQueryable.Where(m => m.Title.Contains(movieFilterDTO.Title));

            if (movieFilterDTO.InCinemas)
                moviesQueryable = moviesQueryable.Where(m => m.InCinemas);

            if (movieFilterDTO.UpComingReleases)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(m => m.ReleaseDate > today);
            }

            if (movieFilterDTO.GenreId != 0)
                moviesQueryable = moviesQueryable
                                    .Where(m => m.Genres.Select(g => g.Id).Contains(movieFilterDTO.GenreId));

            var movies = await moviesQueryable.Include(m => m.Genres).ToListAsync();

            return _mapper.Map<List<MovieDTO>>(movies);
        }

    }
}