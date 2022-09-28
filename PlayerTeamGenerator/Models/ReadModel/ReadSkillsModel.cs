namespace PlayerTeamGenerator.Models.ReadModel
{
    public class ReadSkillsModel
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string Skill { get; set; }
        public int Value { get; set; }
    }
}
