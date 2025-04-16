// Core/Interfaces/IResumeParser.cs
using Core.Entities;


namespace Core.Interfaces
{
    public interface IResumeParser
    {
        Task<Resume> ParseAsync(string plainTextCv);
    }
}
