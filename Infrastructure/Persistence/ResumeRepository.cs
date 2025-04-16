using Core.Common;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ResumeRepository : IResumeRepository
    {
        private readonly ResumeDbContext _context;

        public ResumeRepository(ResumeDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Resume resume)
        {
            _context.Resumes.Add(resume);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Resume>> GetAllAsync()
        {
            return await _context.Resumes.Include(r => r.Experience).ToListAsync();
        }

        public async Task<Resume?> GetByIdAsync(int id)
        {
            return await _context.Resumes
                .Include(r => r.Experience)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task UpdateAsync(Resume resume)
        {
            _context.Resumes.Update(resume);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Resume resume)
        {
            _context.Resumes.Remove(resume);
            await _context.SaveChangesAsync();
        }

        // This method is used to get a paged result of resumes. (for pagination Implementation)
        public async Task<PagedResult<Resume>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Resumes.Include(r => r.Experience).AsQueryable();
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Resume>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

       public async Task<CursorPagedResult<Resume>> GetAfterCursorAsync(
            int? lastSeenId,
            int pageSize,
            string? name = null,
            string? email = null)
        {
            var query = _context.Resumes
                .Include(r => r.Experience)
                .OrderBy(r => r.Id)
                .AsQueryable();

            if (lastSeenId.HasValue)
                query = query.Where(r => r.Id > lastSeenId.Value);

            if (!string.IsNullOrWhiteSpace(name))
                query = query.Where(r => r.Name.ToLower().Contains(name.ToLower()));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(r => r.Email.ToLower().Contains(email.ToLower()));

  

            var items = await query.Take(pageSize + 1).ToListAsync();

            var hasMore = items.Count > pageSize;
            var resultItems = items.Take(pageSize).ToList();
            var nextCursor = hasMore ? resultItems.Last().Id : (int?)null;

            return new CursorPagedResult<Resume>
            {
                Items = resultItems,
                PageSize = pageSize,
                LastSeenId = lastSeenId,
                NextCursor = nextCursor,
                HasMore = hasMore
            };
        }




    }
}
