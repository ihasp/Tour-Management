using AutoMapper;
using ToursNew.Models;
using ToursNew.ViewModels;

    public class ReservationsProfile : Profile
    {
    public ReservationsProfile()
    {
        CreateMap<ReservationViewModel, Reservation>();
        CreateMap<Reservation, ReservationViewModel>();
    }
    }
