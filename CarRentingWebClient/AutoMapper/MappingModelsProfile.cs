using AutoMapper;
using BusinessObjects;
using BusinessObjects.DTOs;
using CarRentingWebClient.Models;

namespace CarRentingWebClient.AutoMapper;

public class MappingModelsProfile : Profile
{
    public MappingModelsProfile()
    {
        CreateMap<RegisterModel, CustomerCreateDTO>().ReverseMap();
        CreateMap<LoginModel, LoginDTO>().ReverseMap();
        CreateMap<RentingDate, RentingDateDTO>().ReverseMap();
    }
}
