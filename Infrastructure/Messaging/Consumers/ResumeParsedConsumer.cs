using Core.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

public class ResumeParsedConsumer : IConsumer<IResumeParsed>
{
    private readonly ILogger<ResumeParsedConsumer> _logger;

    public ResumeParsedConsumer(ILogger<ResumeParsedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<IResumeParsed> context)
    {
        var message = context.Message;
        _logger.LogInformation("🧾 ResumeParsed received: {Id}, {Email}", message.ResumeId, message.Email);
        return Task.CompletedTask;
    }
}
