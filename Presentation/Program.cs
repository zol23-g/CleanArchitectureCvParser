using Application.Services;
using Core.Interfaces;
using Infrastructure.AI;
using Infrastructure.File;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register infrastructure & application services
builder.Services.AddScoped<IFileTextExtractor, FileTextExtractor>();
builder.Services.AddScoped<IResumeParser>(sp =>
    new CohereResumeParser("CRF5ymrXrqJ2jmt7ZZIcmGmsX2qEbUuK5lHFBpQ6"));
builder.Services.AddScoped<IResumeRepository, ResumeRepository>();
builder.Services.AddScoped<ResumeService>();

// Register EF Core with PostgreSQL
builder.Services.AddDbContext<ResumeDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controller and Swagger services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger UI and middleware
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();
