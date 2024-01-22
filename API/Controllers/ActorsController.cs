using API.Data;
using API.DTOs;
using API.Entities;
using API.Utilities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/actors")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public ActorsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ActorDTO>> Get()
        {
            return await _dbContext.Actors.AsNoTracking()
                .OrderBy(g => g.Name)
                .ProjectTo<ActorDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ActorCreationDTO actorCreationDTO)
        {
            var actor = _mapper.Map<Actor>(actorCreationDTO);

            _dbContext.Actors.Add(actor);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(ActorCreationDTO actorCreationDTO, int id)
        {
            var actorDB = await _dbContext.Actors.FirstOrDefaultAsync(a => a.Id == id);

            if (actorDB is null)
                return NotFound();

            actorDB = _mapper.Map(actorCreationDTO, actorDB);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("disconnected/{id:int}")]
        public async Task<ActionResult> PutDisconnected(ActorCreationDTO actorCreationDTO, int id)
        {
            var existsActor = await _dbContext.Actors.AnyAsync(a => a.Id == id);

            if (!existsActor)
                return NotFound();

            var actor = _mapper.Map<Actor>(actorCreationDTO);
            actor.Id = id;

            _dbContext.Update(actor);
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}