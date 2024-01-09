using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class CinemaHall
    {
        public int Id { get; set; }
        public CinemaHallType CinemaHallType { get; set; }
        public decimal Cost { get; set; }
        public int CinemaId { get; set; }
        public Cinema Cinema { get; set; }
        public HashSet<Movie> Movies { get; set; }
    }
}