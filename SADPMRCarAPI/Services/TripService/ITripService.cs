using SADPMRCarAPI.DTO.TripDto;

namespace SADPMRCarAPI.Services.TripService
{
    public interface ITripService
    {
        TripOutputDto CreateTrip(string userEmail, TripInputDto tripInputDto );
        TripOutputDto GetTrip(int id);
        TripOutputDto UpdateTrip(int id, TripInputDto tripInputDto);
        void DeleteTrip(int id);

    }
}
