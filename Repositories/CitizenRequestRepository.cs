using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
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
                .Include(cr => cr.District)
                .AsNoTracking()
                .FirstOrDefaultAsync(cr => cr.Id == id);
        }

        public async Task<IEnumerable<CitizenRequest>> GetAllWithDetailsAsync()
        {
            return await _context.CitizenRequests
                .Include(cr => cr.Citizen)
                .Include(cr => cr.RequestType)
                .Include(cr => cr.RequestStatus)
                .Include(cr => cr.Category)
                .Include(cr => cr.AcceptedBy)
                .Include(cr => cr.AssignedTo)
                .Include(cr => cr.District)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<CitizenRequest>> GetRequestsByStatusAsync(int statusId)
        {
            return await _context.CitizenRequests
                .Where(cr => cr.RequestStatusId == statusId)
                .Include(cr => cr.Citizen)
                .Include(cr => cr.RequestStatus)
                .Include(cr => cr.District)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<CitizenRequest>> GetRequestsByCitizenAsync(int citizenId)
        {
            return await _context.CitizenRequests
                .Where(cr => cr.CitizenId == citizenId)
                .Include(cr => cr.RequestType)
                .Include(cr => cr.RequestStatus)
                .Include(cr => cr.District)
                .AsNoTracking()
                .ToListAsync();
        }

     
        public async Task<IEnumerable<CitizenRequest>> GetAllWithBasicDetailsAsync()
        {
            var requests = await _context.CitizenRequests
                .Include(r => r.Citizen)
                .Include(r => r.RequestType)
                .Include(r => r.RequestStatus)
                .Include(r => r.Category)
                .Include(r => r.AcceptedBy)
                .Include(r => r.AssignedTo)
                .Include(r => r.District)
                .AsNoTracking()
                .ToListAsync();

         
            return requests;
        }
    }
}
