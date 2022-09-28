using System.ComponentModel.DataAnnotations;

namespace PlayerTeamGenerator.Entities;

public class Player
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public List<PlayerSkill> PlayerSkills { get; set; }
}