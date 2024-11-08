using AutoMapper;
using ToursNew.Models;
using ToursNew.ViewModels;

public class ClientsProfile : Profile
{
    public ClientsProfile()
    {
        CreateMap<ClientViewModel, Client>();
        CreateMap<Client, ClientViewModel>();
    }
}