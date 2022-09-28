using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PlayerTeamGenerator.Entities
{
    public class PlayerSkill
    {
        [Key]
        public int Id { get; set; }
        public string Skill { get; set; }
        public int Value { get; set; }

        [ForeignKey("Player")]
        public int PlayerId { get; set; }
        [JsonIgnore]
        public Player Player { get; set; }
    }
}
