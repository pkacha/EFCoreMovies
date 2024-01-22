using API.DTOs;
using API.Entities;
using AutoMapper;
using NetTopologySuite;
using NetTopologySuite.Geometries;

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

            CreateMap<ActorCreationDTO, Actor>();

            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
            CreateMap<CinemaCreationDTO, Cinema>()
                .ForMember(ent => ent.Location, dto
                    => dto.MapFrom(prop => geometryFactory.CreatePoint(new Coordinate(prop.Longitude, prop.Latitude))));

            CreateMap<CinemaOfferCreationDTO, CinemaOffer>();
            CreateMap<CinemaHallCreationDTO, CinemaHall>();

            CreateMap<Genre, GenreCreationDTO>();
            CreateMap<GenreCreationDTO, Genre>();

            CreateMap<Movie, MovieDTO>()
                .ForMember(dto => dto.Genres, ent => ent.MapFrom(g => g.Genres.OrderByDescending(gn => gn.Name)))
                .ForMember(dto => dto.Cinemas, ent =>
                    ent.MapFrom(p => p.CinemaHalls.OrderByDescending(ch => ch.Cinema.Name).Select(c => c.Cinema)))
                .ForMember(dto => dto.Actors, ent => ent.MapFrom(p => p.MoviesActors.Select(a => a.Actor)));

            CreateMap<MovieCreationDTO, Movie>()
                .ForMember(ent => ent.Genres, dto => dto.MapFrom(prop => prop.GenreIds.Select(id => new Genre() { Id = id })))
                .ForMember(ent => ent.CinemaHalls, dto => dto.MapFrom(prop => prop.CinemaHallIds.Select(id => new CinemaHall() { Id = id })));

            CreateMap<MovieActorCreationDTO, MovieActor>();
        }
    }
}