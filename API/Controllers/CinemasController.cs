using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Entities;
using API.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using API.DTOs;
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace API.Controllers
{
    [ApiController]
    [Route("api/cinemas")]
    public class CinemasController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public CinemasController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IEnumerable<CinemaDTO>> Get()
        {
            return await _dbContext.Cinemas
                .ProjectTo<CinemaDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpGet("closetome")]
        public async Task<ActionResult> Get(double latitude, double longitude)
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var mylocation = geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
            var maxDistanceInMeters = 2000; // 2 kms

            var cinemas = await _dbContext.Cinemas
                .Where(c => c.Location.IsWithinDistance(mylocation, maxDistanceInMeters))
                .OrderBy(c => c.Location.Distance(mylocation))
                .Select(c => new
                {
                    Name = c.Name,
                    Distance = Math.Round(c.Location.Distance(mylocation))
                }).ToListAsync();

            return Ok(cinemas);
        }

        [HttpPost()]
        public async Task<ActionResult> Post()
        {
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            var cinemaLocation = geometryFactory.CreatePoint(new Coordinate(-34.069025, 18.843406));

            var cinema = new Cinema()
            {
                Name = "BMG Cinema",
                Location = cinemaLocation, //"15 Smuts Ave, Briza Twp, Cape Town, 7130, South Africa",
                CinemaOffer = new CinemaOffer()
                {
                    DiscountPercentage = 5,
                    Begin = DateTime.Today,
                    End = DateTime.Today.AddDays(7)
                },
                CinemaHalls = new HashSet<CinemaHall>()
                 {
                    new CinemaHall()
                    {
                        Cost = 200,
                        CinemaHallType = CinemaHallType.TwoDimensions
                    },
                     new CinemaHall()
                    {
                        Cost = 300,
                        CinemaHallType = CinemaHallType.ThreeDimensions
                    }
                 }
            };

            _dbContext.Add(cinema);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("withDTO")]
        public async Task<ActionResult> Post(CinemaCreationDTO cinemaCreationDTO)
        {
            var cinema = _mapper.Map<Cinema>(cinemaCreationDTO);

            _dbContext.Cinemas.Add(cinema);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult> Get(int id)
        {
            var cinemaDB = await _dbContext.Cinemas
                   .Include(c => c.CinemaHalls)
                   .Include(c => c.CinemaOffer)
                   .FirstOrDefaultAsync(c => c.Id == id);

            if (cinemaDB is null)
                return NotFound();

            cinemaDB.Location = null;

            return Ok(cinemaDB);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(CinemaCreationDTO cinemaCreationDTO, int id)
        {
            var cinemaDB = await _dbContext.Cinemas
                    .Include(c => c.CinemaHalls)
                    .Include(c => c.CinemaOffer)
                    .FirstOrDefaultAsync(c => c.Id == id);

            if (cinemaDB is null)
                return NotFound();

            cinemaDB = _mapper.Map(cinemaCreationDTO, cinemaDB);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}