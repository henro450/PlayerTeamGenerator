namespace PlayerTeamGenerator.Models.ReadModel
{
    public class ReadPlayersModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public List<ReadSkillsModel> PlayerSkills { get; set; }
    }
}
