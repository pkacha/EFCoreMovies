using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/people")]
    public class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public PeopleController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Person>> Get(int id)
        {
            var person = await _dbContext.People
                .Include(p => p.ReceivedMessages)
                .Include(p => p.SentMessages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (person is null)
                return NotFound();

            return person;
        }
    }
}