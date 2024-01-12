using API.DTOs;
using API.Entities;
using AutoMapper;

namespace API.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Actor, ActorDTO>();

            CreateMap<Cinema, CinemaDTO>()
                .ForMember(dto => dto.Latitude, ent => ent.MapFrom(p => p.Location.Y))
                .ForMember(dto => dto.Longitude, ent => ent.MapFrom(p => p.Location.X));

            CreateMap<Genre, GenreDTO>();

            CreateMap<Movie, MovieDTO>()
                .ForMember(dto => dto.Genres, ent => ent.MapFrom(g => g.Genres.OrderByDescending(gn => gn.Name)))
                .ForMember(dto => dto.Cinemas, ent =>
                    ent.MapFrom(p => p.CinemaHalls.OrderByDescending(ch => ch.Cinema.Name).Select(c => c.Cinema)))
                .ForMember(dto => dto.Actors, ent => ent.MapFrom(p => p.MovieActors.Select(a => a.Actor)));
        }
    }
}