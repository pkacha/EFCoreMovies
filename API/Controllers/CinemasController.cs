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
    }
}