using PlayerTeamGenerator.Entities;
using PlayerTeamGenerator.Helpers;

namespace PlayerTeamGenerator.Framework.Application
{
    public interface IPlayerManager
    {
        Task<ManagerResponse<long>> CreateAsync(Player player);
        Task<ManagerResponse<Player>> Get(long playerId);
        Task<ManagerResponse<bool>> DeleteAsync(Player player);
        Task<ManagerResponse<List<Player>>> Get();
        Task<ManagerResponse<long>> UpdateAsync(Player player);
    }
}
