using Microsoft.AspNetCore.Identity;
using SADPMRCarAPI.DTO.CarAccessoriesDto;
using SADPMRCarAPI.DTO.CarDto;
using SADPMRCarAPI.DTO.CarServiceDto;
using SADPMRCarAPI.DTO.RegisterDto;
using SADPMRCarAPI.DTO.TripDto;
using SADPMRCarAPI.DTO.UserDto.UserDto;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Automapper
{
    public class RentalProfile : Profile
    {
        public RentalProfile() 
        {
            // Mapping for the trip
            CreateMap<TripInputDto, TripModel>();
            CreateMap<TripModel, TripOutputDto>();

            //for registration
            CreateMap<RegisterDto, ApplicationUserIdentity>();
            CreateMap<ApplicationUserIdentity, RegisterDto>();

            // Mapping for CarModel
            CreateMap<CarDto, CarModel>().ReverseMap();
            // Mapping for CarService 
            CreateMap<CarServiceModel, AddCarServiceDto>().ReverseMap();

            // Mapping for CarAccesories 
            CreateMap<CarAccessories, AddCarAccessoriesDto>().ReverseMap();


            // Mapping for UserDto and ApplicationUserIdentity
            CreateMap<UserDto, ApplicationUserIdentity>().ReverseMap();

  

        }
    }
}
