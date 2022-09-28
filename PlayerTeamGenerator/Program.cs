using PlayerTeamGenerator.Framework.Application;
using PlayerTeamGenerator.Framework.Infrastructure;
using PlayerTeamGenerator.Helpers;
using PlayerTeamGenerator.Models.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddControllers();
services.AddScoped<IPlayerManager, PlayerManager>();
services.AddScoped<ITeamManager, TeamManager>();
services.AddScoped<UnitOfWork>();

services.AddAutoMapper(options =>
{
}, typeof(AutoMapperProfile));

services.AddSqlite<DataContext>("DataSource=webApi.db");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dataContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }