// Application/Features/Resume/Commands/ParseResumeCommand.cs
using Core.Entities;
using MediatR;

namespace Application.Features.Resumes.Commands
{
    public record ParseResumeCommand(byte[] FileBytes, string FileName) : IRequest<Resume>;
}
