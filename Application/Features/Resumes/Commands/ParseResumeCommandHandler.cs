
// Application/Features/Resumes/Commands/ParseResumeCommandHandler.cs
using Core.Entities;
using Core.Interfaces;
using MediatR;
using MassTransit;
using Core.Events;

namespace Application.Features.Resumes.Commands
{
    public class ParseResumeCommandHandler : IRequestHandler<ParseResumeCommand, Resume>
    {
        private readonly IFileTextExtractor _textExtractor;
        private readonly IResumeParser _parser;
        private readonly IResumeRepository _repository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ParseResumeCommandHandler(
            IFileTextExtractor textExtractor,
            IResumeParser parser,
            IResumeRepository repository,
            IPublishEndpoint publishEndpoint)
        {
            _textExtractor = textExtractor;
            _parser = parser;
            _repository = repository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Resume> Handle(ParseResumeCommand request, CancellationToken cancellationToken)
        {
            using var stream = new MemoryStream(request.FileBytes);

            string plainText = request.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)
                ? await _textExtractor.ExtractPdfAsync(stream)
                : request.FileName.EndsWith(".docx", StringComparison.OrdinalIgnoreCase)
                    ? await _textExtractor.ExtractDocxAsync(stream)
                    : throw new NotSupportedException("Only .pdf and .docx files are supported.");

            var resume = await _parser.ParseAsync(plainText);
            await _repository.AddAsync(resume);

            // Use event contract defined in Application
            await _publishEndpoint.Publish<IResumeParsed>(new
            {
                ResumeId = resume.Id,
                Email = resume.Email
            }, cancellationToken);

            return resume;
        }
    }
}
