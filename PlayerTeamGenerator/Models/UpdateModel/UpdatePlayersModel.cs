namespace PlayerTeamGenerator.Models.UpdateModel
{
    public class UpdatePlayersModel
    {
        public string Name { get; set; }
        public string Position { get; set; }

        public List<UpdateSkillsModel> PlayerSkills { get; set; }
    }
}
