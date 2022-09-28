using PlayerTeamGenerator.Entities;
using PlayerTeamGenerator.Helpers;
using PlayerTeamGenerator.Models.RequestModel;

namespace PlayerTeamGenerator.Framework.Application
{
    public interface ITeamManager
    {
        Task<ManagerResponse<List<Player>>> GetPlayersByPositionSkill(RequestPlayerModel selection);
        Task<ManagerResponse<bool>> ValidateRequest(List<RequestPlayerModel> model);
    }
}
