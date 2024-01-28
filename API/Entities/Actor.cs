using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                // tOm hOLLanD => Tom Holland
                _name = string.Join(' ',
                    value.Split(' ')
                    .Select(n => n[0].ToString().ToUpperInvariant() + n.Substring(1).ToLowerInvariant()).ToArray());
            }
        }
        public string Biography { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [NotMapped]
        public int? Age
        {
            get
            {
                if (!DateOfBirth.HasValue) return null;

                var dob = DateOfBirth.Value;
                var today = DateTime.Today;
                var age = today.Year - dob.Year;

                if (new DateTime(today.Year, dob.Month, dob.Day) > today) age--;

                return age;
            }
        }
        public string PictureURL { get; set; }
        public HashSet<MovieActor> MovieActors { get; set; }
        public Address Address { get; set; }
    }
}