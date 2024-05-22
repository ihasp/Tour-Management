using AutoMapper;
using ToursNew.Models;
using ToursNew.ViewModels;

    public class TripsProfile : Profile
    {
    public TripsProfile()
    {
        CreateMap<TripViewModel, Trip>();
        CreateMap<Trip, TripViewModel>();
    }
    }
