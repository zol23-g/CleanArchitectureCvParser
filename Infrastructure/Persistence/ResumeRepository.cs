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
    }
}
