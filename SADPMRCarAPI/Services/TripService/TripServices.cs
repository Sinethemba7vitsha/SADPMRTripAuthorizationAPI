using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Exchange.WebServices.Data;
using SADPMRCarAPI.Data;
using SADPMRCarAPI.DTO.TripDto;
using SADPMRCarAPI.Model;
using System.Globalization;
using System.Security.Claims;

namespace SADPMRCarAPI.Services.TripService
{
    public class TripService : ITripService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public TripService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public TripOutputDto CreateTrip(string userEmail, TripInputDto tripInputDto)
        {
            var tripModel = new TripModel
            {
                NameOfCoDriver = tripInputDto.NameOfCoDriver,
                TimeOut = DateTime.ParseExact(tripInputDto.TimeOut, "HH:mm", CultureInfo.InvariantCulture),
                ReturnTime = DateTime.ParseExact(tripInputDto.ReturnTime, "HH:mm", CultureInfo.InvariantCulture),
                Date = DateTime.ParseExact(tripInputDto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                ReasonForTravel = tripInputDto.ReasonForTravel,
                Destination = tripInputDto.Destination,
                StatusId = 3,
                StatusUpdatedDate = DateTime.Now,
                UserEmail = userEmail,
            };

            _dbContext.Trips.Add(tripModel);
            _dbContext.SaveChanges();
            var createdTripDto = _mapper.Map<TripOutputDto>(tripModel);
            return createdTripDto;
        }

        public TripOutputDto? GetTrip(int id)
        {
            var tripModel = _dbContext.Trips.FirstOrDefault(t => t.TripId == id);

            if (tripModel == null)
            {
                return null;
            }

            var tripDto = _mapper.Map<TripOutputDto>(tripModel);

            return tripDto;
        }

        public TripOutputDto UpdateTrip(int id, TripInputDto tripInputDto)
        {
            var tripModel = _dbContext.Trips.FirstOrDefault(t => t.TripId == id);

            if (tripModel == null)
            {
                return null;
            }

            tripModel.NameOfCoDriver = tripInputDto.NameOfCoDriver;
            tripModel.TimeOut = DateTime.ParseExact(tripInputDto.TimeOut, "HH:mm", CultureInfo.InvariantCulture);
            tripModel.ReturnTime = DateTime.ParseExact(tripInputDto.ReturnTime, "HH:mm", CultureInfo.InvariantCulture);
            tripModel.Date = DateTime.ParseExact(tripInputDto.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            tripModel.ReasonForTravel = tripInputDto.ReasonForTravel;
            tripModel.Destination = tripInputDto.Destination;

            _dbContext.SaveChanges();

            var updatedTripDto = _mapper.Map<TripOutputDto>(tripModel);

            return updatedTripDto;
        }

        public void DeleteTrip(int id)
        {
            var tripModel = _dbContext.Trips.FirstOrDefault(t => t.TripId == id);

            if (tripModel != null)
            {
                _dbContext.Trips.Remove(tripModel);
                _dbContext.SaveChanges();
            }
        }

    }
}

