using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace API.Entities
{
    [Owned]
    public class Address
    {
        public string Street { get; set; }
        public string Province { get; set; }
        [Required]
        public string Country { get; set; }
    }
}