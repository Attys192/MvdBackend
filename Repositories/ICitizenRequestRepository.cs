using MvdBackend.Models;

namespace MvdBackend.Repositories
{
    public interface ICitizenRequestRepository : IRepository<CitizenRequest>
    {
        Task<CitizenRequest> GetWithDetailsAsync(int id);
        Task<IEnumerable<CitizenRequest>> GetRequestsByStatusAsync(int statusId);
        Task<IEnumerable<CitizenRequest>> GetRequestsByCitizenAsync(int citizenId);
        Task<IEnumerable<CitizenRequest>> GetAllWithBasicDetailsAsync();
    }
}
