namespace PlayerTeamGenerator.Helpers;

using Microsoft.EntityFrameworkCore;
using PlayerTeamGenerator.Entities;

public class DataContext : DbContext
{
  public DataContext() { }

  public DataContext(DbContextOptions<DataContext> options)
       : base(options) { }

  public DbSet<Player> Players { get; set; }
  public DbSet<PlayerSkill> PlayerSkills { get; set; }
}