// Core/Interfaces/IResumeParser.cs
using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IResumeParser
    {
        Task<Resume> ParseAsync(string plainTextCv);
    }
}
