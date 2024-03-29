using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public HashSet<MovieActor> MovieActors { get; set; }
        public Address BillingAddress { get; set; }
        public Address HomeAddress { get; set; }
    }
}