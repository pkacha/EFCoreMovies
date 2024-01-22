namespace API.DTOs
{
    public class MovieCreationDTO
    {
        public string Title { get; set; }
        public bool InCinemas { get; set; }
        public DateTime ReleaseDate { get; set; }
        public List<int> GenreIds { get; set; }
        public List<int> CinemaHallIds { get; set; }
        public List<MovieActorCreationDTO> MoviesActors { get; set; }

    }
}