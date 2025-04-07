using Application.Services;
using Core.Interfaces;
using Infrastructure.AI;
using Infrastructure.File;
var builder = WebApplication.CreateBuilder(args);

// Register dependencies
builder.Services.AddScoped<IFileTextExtractor, FileTextExtractor>();
builder.Services.AddScoped<IResumeParser>(sp =>
    new CohereResumeParser("CRF5ymrXrqJ2jmt7ZZIcmGmsX2qEbUuK5lHFBpQ6"));

// builder.Services.AddScoped<ResumeService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
