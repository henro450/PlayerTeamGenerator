using AutoMapper;
using PlayerTeamGenerator.Entities;
using PlayerTeamGenerator.Models.CreateModel;
using PlayerTeamGenerator.Models.ReadModel;
using PlayerTeamGenerator.Models.UpdateModel;

namespace PlayerTeamGenerator.Models.Mapping
{
    public class PlayerMapping : Profile
    {
        public PlayerMapping()
        {
            CreateMap<CreatePlayersModel, Player>();
            CreateMap<Player, ReadPlayersModel>();
            CreateMap<UpdatePlayersModel, Player>().ReverseMap();

            CreateMap<CreateSkillsModel, PlayerSkill>();
            CreateMap<PlayerSkill, ReadSkillsModel>();
            CreateMap<UpdateSkillsModel, PlayerSkill>().ReverseMap();
        }
    }
}
