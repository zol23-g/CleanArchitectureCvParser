// Application/Features/Resume/Queries/GetResumeByIdQueryHandler.cs

using Core.Entities;
using Core.Interfaces;
using MediatR;
using Application.Features.Resumes.Queries;

namespace Application.Features.Resumes.Handlers
{
    public class GetResumeByIdQueryHandler : IRequestHandler<GetResumeByIdQuery, Resume?>
    {
        private readonly IResumeRepository _repository;

        public GetResumeByIdQueryHandler(IResumeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Resume?> Handle(GetResumeByIdQuery request, CancellationToken cancellationToken)
        {
            var resume = await _repository.GetByIdAsync(request.Id);
            return resume;
        }
    }
}
