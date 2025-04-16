using Core.Entities;
using Core.Common;

namespace Core.Interfaces
{
    public interface IResumeRepository
    {
        Task AddAsync(Resume resume);
        Task<List<Resume>> GetAllAsync();

        Task<Resume?> GetByIdAsync(int id);
        Task UpdateAsync(Resume resume);
        Task DeleteAsync(Resume resume);

        // This method is used to get a paged result of resumes.(for pagination in a UI)
        // It returns a PagedResult<Resume> object that contains the resumes for the specified page and size.
        Task<PagedResult<Resume>> GetPagedAsync(int pageNumber, int pageSize);

        // This method is used to get a paged result of resumes using a cursor-based approach.
        Task<CursorPagedResult<Resume>> GetAfterCursorAsync(
        int? lastSeenId,
        int pageSize,
        string? name = null,
        string? email = null
        
        );





    }
}
