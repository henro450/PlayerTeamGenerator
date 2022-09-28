using Microsoft.EntityFrameworkCore;
using PlayerTeamGenerator.Entities;
using PlayerTeamGenerator.Framework.Application;
using PlayerTeamGenerator.Helpers;
using PlayerTeamGenerator.Models.RequestModel;

namespace PlayerTeamGenerator.Framework.Infrastructure
{
    public class TeamManager : ITeamManager
    {
        private readonly UnitOfWork unitOfWork;

        public TeamManager(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ManagerResponse<List<Player>>> GetPlayersByPositionSkill(RequestPlayerModel selection)
        {
            //Get players and filter by position
            var players = await unitOfWork.Players.Get(x => x.Position == selection.Position)
                .Include(x => x.PlayerSkills)
                .ToListAsync();

            if (!players.Any())
            {
                return ManagerResponse<List<Player>>.Failure
                    ($"No player matches your request: Position - {selection.Position}");
            }

            //Check if list of players is enough for selection
            if (selection.NumberOfPlayers > players.Count())
            {
                return ManagerResponse<List<Player>>.Failure($"Insufficient number of players for position: {selection.Position}");
            }

            //Filter Players with highest skill value
            var playerTeam = HighestSkillPlayers(players, selection.MainSkill, selection.NumberOfPlayers);

            //Check if list of players is enough for selection
            if (selection.NumberOfPlayers > playerTeam.Count())
            {
                return ManagerResponse<List<Player>>.Failure($"Insufficient number of players for position: {selection.Position}");
            }

            return ManagerResponse<List<Player>>.Success(playerTeam);
        }

        public List<Player> HighestSkillPlayers(List<Player> players, string skill, int numberOfPlayers)
        {
            //Get position filtered players and fillter by skill
            var playerFilter = players.Where(x => x.PlayerSkills.Any(y => y.Skill == skill)).ToList();

            var skilledPlayers = new List<Player>();
         
            //Get list of skills from players and filter by hightest skill value
            var skills = playerFilter.SelectMany(x => x.PlayerSkills
                .Where(y => y.Skill == skill))
                .OrderByDescending(x => x.Value).ToList();

            //Choose the required amount of selection
            var skills1 = skills.Take(numberOfPlayers);

            //Retrieve players for selected skills
            foreach (var s in skills1)
            {
                var selectedSkillPlayer = players.Where(x => x.Id == s.PlayerId).FirstOrDefault();
                skilledPlayers.Add(selectedSkillPlayer);
            }

            //Add more players by position if result is less than the number of players requested
            if (skilledPlayers.Count() < numberOfPlayers)
            {
                var abc = AdditionPlayers(players, (numberOfPlayers - skilledPlayers.Count()));
                skilledPlayers.AddRange(abc);
            }

            return skilledPlayers;
        }

        public async Task<ManagerResponse<bool>> ValidateRequest(List<RequestPlayerModel> model)
        {
            var checkRepeatition = CheckSkillAndPositionRepeatition(model);
            if (!checkRepeatition)
            {
                return ManagerResponse<bool>.Failure("Only one skill is needed for a position");
            }

            var checkPositionRepeatition = CheckPositionRepeatition(model);
            if (!checkPositionRepeatition)
            {
                return ManagerResponse<bool>.Failure("You cannot search for more than one of same position");
            }

            return ManagerResponse<bool>.Success();
        }

        //Checks for validations in the request
        private bool CheckSkillAndPositionRepeatition(List<RequestPlayerModel> model)
        {
            foreach (var request in model)
            {
                var check = model.Where(x => x.Position == request.Position && x.MainSkill == request.MainSkill);

                if (check != null && check.Count() > 1)
                {
                    return false;
                }
            }

            return true;
        }

        //Checks for repeated position in the request
        private bool CheckPositionRepeatition(List<RequestPlayerModel> model)
        {
            var positions = model.GroupBy(x => new { x.Position })
                   .Where(x => x.Skip(1).Any());
            
            if (positions.Count() > 0)
            {
                return false;
            }

            return true;
        }

        private List<Player> AdditionPlayers(List<Player> players, int expected)
        {
            var skilledPlayers = new List<Player>();

            //Get list of skills from players and filter by hightest skill value
            var skills = players.SelectMany(x => x.PlayerSkills
                .OrderByDescending(x => x.Value))
                .DistinctBy(z => z.PlayerId)
                .ToList();

            //Choose the required amount of selection
            if (expected < skills.Count())
            {
                skills = skills.Take(expected).ToList();
            }

            //Retrieve players for selected skills
            foreach (var s in skills)
            {
                var selectedSkillPlayer = players.Where(x => x.Id == s.PlayerId).FirstOrDefault();
                skilledPlayers.Add(selectedSkillPlayer);
            }

            return skilledPlayers;
        }
    }
}
