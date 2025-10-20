using Microsoft.EntityFrameworkCore;
using MvdBackend.Data;
using MvdBackend.Models;

namespace MvdBackend.Repositories
{
    public class CitizenRequestRepository : Repository<CitizenRequest>, ICitizenRequestRepository
    {
        public CitizenRequestRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<CitizenRequest> GetWithDetailsAsync(int id)
        {
            return await _context.CitizenRequests
                .Include(cr => cr.Citizen)
                .Include(cr => cr.RequestType)
                .Include(cr => cr.RequestStatus)
                .Include(cr => cr.AcceptedBy)
                .Include(cr => cr.AssignedTo)
                .Include(cr => cr.Category)
                .FirstOrDefaultAsync(cr => cr.Id == id);
        }

        // ← ДОБАВЬ ЭТОТ МЕТОД ↓
        public async Task<IEnumerable<CitizenRequest>> GetAllWithDetailsAsync()
        {
            return await _context.CitizenRequests
                .Include(cr => cr.Citizen)
                .Include(cr => cr.RequestType)
                .Include(cr => cr.RequestStatus)
                .Include(cr => cr.Category)
                .Include(cr => cr.AcceptedBy)
                .Include(cr => cr.AssignedTo)
                .ToListAsync();
        }

        public async Task<IEnumerable<CitizenRequest>> GetRequestsByStatusAsync(int statusId)
        {
            return await _context.CitizenRequests
                .Where(cr => cr.RequestStatusId == statusId)
                .Include(cr => cr.Citizen)
                .Include(cr => cr.RequestStatus)
                .ToListAsync();
        }

        public async Task<IEnumerable<CitizenRequest>> GetRequestsByCitizenAsync(int citizenId)
        {
            return await _context.CitizenRequests
                .Where(cr => cr.CitizenId == citizenId)
                .Include(cr => cr.RequestType)
                .Include(cr => cr.RequestStatus)
                .ToListAsync();
        }
        public async Task<IEnumerable<CitizenRequest>> GetAllWithBasicDetailsAsync()
        {
            return await _context.CitizenRequests
                .Include(cr => cr.Citizen)
                .Include(cr => cr.RequestType)
                .Include(cr => cr.RequestStatus)
                .Include(cr => cr.Category)
                .Include(cr => cr.AcceptedBy)
                .Include(cr => cr.AssignedTo)
                .Select(cr => new CitizenRequest
                {
                    Id = cr.Id,
                    CitizenId = cr.CitizenId,
                    Citizen = new Citizen { Id = cr.Citizen.Id, LastName = cr.Citizen.LastName, FirstName = cr.Citizen.FirstName, Patronymic = cr.Citizen.Patronymic },
                    RequestTypeId = cr.RequestTypeId,
                    RequestType = new RequestType { Id = cr.RequestType.Id, Name = cr.RequestType.Name },
                    CategoryId = cr.CategoryId,
                    Category = new Category { Id = cr.Category.Id, Name = cr.Category.Name, Description = cr.Category.Description },
                    Description = cr.Description,
                    AcceptedById = cr.AcceptedById,
                    AcceptedBy = new Employee { Id = cr.AcceptedBy.Id, LastName = cr.AcceptedBy.LastName, FirstName = cr.AcceptedBy.FirstName, Patronymic = cr.AcceptedBy.Patronymic },
                    AssignedToId = cr.AssignedToId,
                    AssignedTo = new Employee { Id = cr.AssignedTo.Id, LastName = cr.AssignedTo.LastName, FirstName = cr.AssignedTo.FirstName, Patronymic = cr.AssignedTo.Patronymic },
                    IncidentTime = cr.IncidentTime,
                    CreatedAt = cr.CreatedAt,
                    IncidentLocation = cr.IncidentLocation,
                    CitizenLocation = cr.CitizenLocation,
                    RequestStatusId = cr.RequestStatusId,
                    RequestStatus = new RequestStatus { Id = cr.RequestStatus.Id, Name = cr.RequestStatus.Name }
                })
                .ToListAsync();
        }
    }
}