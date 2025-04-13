using Core.Entities;

namespace Core.Interfaces
{
    public interface IResumeRepository
    {
        Task AddAsync(Resume resume);
        Task<List<Resume>> GetAllAsync();

        Task<Resume?> GetByIdAsync(int id);
        Task UpdateAsync(Resume resume);
        Task DeleteAsync(Resume resume);

    }
}
