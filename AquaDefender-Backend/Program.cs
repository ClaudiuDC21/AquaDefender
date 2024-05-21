using AquaDefender_Backend;
using AquaDefender_Backend.Data;
using AquaDefender_Backend.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AquaDefender API",
        Version = "v1",
        Description = "An app that will help people to protect better water and also to know more about it.",
        Contact = new OpenApiContact
        {
            Name = "Dascalu Claudiu",
            Email = "aquadefender00@gmail.com",
        },
    });

});
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddHttpClient("MailTrapApiClient", (services, client) =>
{
    var mailSettings = services.GetRequiredService<IOptions<MailSettings>>().Value;
    client.BaseAddress = new Uri(mailSettings.ApiBaseUrl);
    client.DefaultRequestHeaders.Add("Api-Token", mailSettings.ApiToken);
});

builder.Services.AddCors();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

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

var env = app.Services.GetRequiredService<IWebHostEnvironment>();
env.WebRootPath = "imagesRoot";

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
