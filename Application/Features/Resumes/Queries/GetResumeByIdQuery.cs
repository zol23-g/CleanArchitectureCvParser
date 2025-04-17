// Application/Features/Resume/Queries/GetResumeByIdQuery.cs

using Core.Entities;
using MediatR;

namespace Application.Features.Resumes.Queries
{
    public record GetResumeByIdQuery(int Id) : IRequest<Resume>;
}
