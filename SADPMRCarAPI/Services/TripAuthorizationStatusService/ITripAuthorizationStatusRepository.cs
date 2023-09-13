using SADPMRCarAPI.DTO.TripAuthorizationStatusDTO;

namespace SADPMRCarAPI.Services.TripAuthorizationStatusService
{
    public interface ITripAuthorizationStatusRepository
    {
        Task<TripAuthorizationStatusDTO> GetById(int id);
        Task<List<TripAuthorizationStatusDTO>> GetAll();
        Task<int> Create(TripAuthorizationStatusDTO tripAuthorizationStatusDTO);
        Task Update(TripAuthorizationStatusDTO tripAuthorizationStatusDTO);
        Task Delete(int id);
    }
}
