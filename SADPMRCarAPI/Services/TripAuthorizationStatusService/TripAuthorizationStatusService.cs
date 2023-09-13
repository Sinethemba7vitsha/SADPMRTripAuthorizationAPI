using Microsoft.EntityFrameworkCore;
using SADPMRCarAPI.DTO.TripAuthorizationStatusDTO;
using SADPMRCarAPI.Model;

namespace SADPMRCarAPI.Services.TripAuthorizationStatusService
{
    public class TripAuthorizationStatusService : ITripAuthorizationStatusRepository
    {
        private readonly DbContext _dbContext;

        public TripAuthorizationStatusService(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TripAuthorizationStatusDTO> GetById(int id)
        {
            var tripAuthorizationStatus = await _dbContext
                .Set<TripAuthorizationStatus>()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tripAuthorizationStatus == null)
            {
                return null;
            }

            return MapToDTO(tripAuthorizationStatus);
        }

        public async Task<List<TripAuthorizationStatusDTO>> GetAll()
        {
            var tripAuthorizationStatusList = await _dbContext
                .Set<TripAuthorizationStatus>()
                .ToListAsync();

            return tripAuthorizationStatusList.Select(MapToDTO).ToList();
        }

        public async Task<int> Create(TripAuthorizationStatusDTO tripAuthorizationStatusDTO)
        {
            var tripAuthorizationStatus = new TripAuthorizationStatus
            {
                Trip_Status = tripAuthorizationStatusDTO.TripStatus,
                ModifiedBy = tripAuthorizationStatusDTO.ModifiedBy
            };

            _dbContext.Set<TripAuthorizationStatus>().Add(tripAuthorizationStatus);
            await _dbContext.SaveChangesAsync();

            return tripAuthorizationStatus.Id;
        }

        public async Task Update(TripAuthorizationStatusDTO tripAuthorizationStatusDTO)
        {
            var existingTripAuthorizationStatus = await _dbContext
                .Set<TripAuthorizationStatus>()
                .FirstOrDefaultAsync(t => t.Id == tripAuthorizationStatusDTO.Id);

            if (existingTripAuthorizationStatus == null)
            {
                throw new ArgumentException("Invalid trip authorization status ID.");
            }

            existingTripAuthorizationStatus.Trip_Status = tripAuthorizationStatusDTO.TripStatus;
            existingTripAuthorizationStatus.ModifiedBy = tripAuthorizationStatusDTO.ModifiedBy;

            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var tripAuthorizationStatus = await _dbContext
                .Set<TripAuthorizationStatus>()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tripAuthorizationStatus == null)
            {
                throw new ArgumentException("Invalid trip authorization status ID.");
            }

            _dbContext.Set<TripAuthorizationStatus>().Remove(tripAuthorizationStatus);
            await _dbContext.SaveChangesAsync();
        }

        private TripAuthorizationStatusDTO MapToDTO(TripAuthorizationStatus tripAuthorizationStatus)
        {
            return new TripAuthorizationStatusDTO
            {
                Id = tripAuthorizationStatus.Id,
                TripStatus = tripAuthorizationStatus.Trip_Status,
                ModifiedBy = tripAuthorizationStatus.ModifiedBy
            };
        }
    }

}
