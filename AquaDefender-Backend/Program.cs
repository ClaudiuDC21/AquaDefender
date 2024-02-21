using AquaDefender_Backend.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<AquaDefenderDataContext>(options =>
    options.UseSqlServer(@"Server=ASUS-LAPTOPCLAU\SQLEXPRESS;Database=AquaDefenderDatabase;Trusted_Connection=True;Encrypt=False;"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins("http://localhost:4200"));

app.MapControllers();

app.Run();
