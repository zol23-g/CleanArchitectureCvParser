using Core.Entities;

namespace Core.Interfaces
{
    public interface IResumeRepository
    {
        Task AddAsync(Resume resume);
        Task<List<Resume>> GetAllAsync();
    }
}
