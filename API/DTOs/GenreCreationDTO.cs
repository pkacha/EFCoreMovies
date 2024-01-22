using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class GenreCreationDTO
    {
        [Required]
        public string Name { get; set; }
    }
}