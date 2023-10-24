using AutoMapper;
using BusinessObjects.DTOs;

namespace BusinessObjects.AutoMapper;

public class MappingDTOsProfile : Profile
{
    public MappingDTOsProfile()
    {
        CreateMap<Customer, CustomerCreateDTO>().ReverseMap();
        CreateMap<CarInformation, CarCreateDTO>().ReverseMap();
        CreateMap<RentingTransaction, RentingCreateDTO>().ReverseMap();
        CreateMap<RentingDetail, RentingDetailCreateDTO>().ReverseMap();
        CreateMap<Customer, Account>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CustomerId));
    }
}
