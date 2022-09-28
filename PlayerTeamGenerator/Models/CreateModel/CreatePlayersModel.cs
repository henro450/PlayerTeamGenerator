namespace PlayerTeamGenerator.Models.CreateModel
{
    public class CreatePlayersModel
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public List<CreateSkillsModel> PlayerSkills { get; set; }
    }
}
