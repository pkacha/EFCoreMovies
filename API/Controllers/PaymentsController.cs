using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public PaymentsController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> Get()
        {
            return await _dbContext.Payments.ToListAsync();
        }

        [HttpGet("cards")]
        public async Task<ActionResult<IEnumerable<CardPayment>>> GetCardPayments()
        {
            return await _dbContext.Payments.OfType<CardPayment>().ToListAsync();
        }

        [HttpGet("paypal")]
        public async Task<ActionResult<IEnumerable<PayPalPayment>>> GetPayPalPayments()
        {
            return await _dbContext.Payments.OfType<PayPalPayment>().ToListAsync();
        }
    }
}