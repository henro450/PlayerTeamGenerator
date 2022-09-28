using Microsoft.EntityFrameworkCore;
using PlayerTeamGenerator.Entities;
using PlayerTeamGenerator.Framework.Application;
using PlayerTeamGenerator.Helpers;

namespace PlayerTeamGenerator.Framework.Infrastructure
{
    public class PlayerManager : IPlayerManager
    {
        private readonly UnitOfWork unitOfWork;

        public PlayerManager(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ManagerResponse<long>> CreateAsync(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            try
            {
                var result = unitOfWork.Players.Add(player);

                await unitOfWork.SaveChangesAsync();

                return ManagerResponse<long>.Success(result.Id);
            }
            catch (Exception ex)
            {
                return ManagerResponse<long>.Failure($"Error Creating Player: {player.Name}");
            }

        }

        public async Task<ManagerResponse<Player>> Get(long playerId)
        {
            try
            {
                var player = await unitOfWork.Players.Get(x => x.Id == playerId)
                    .Include(x => x.PlayerSkills)
                    .FirstOrDefaultAsync();

                return ManagerResponse<Player>.Success(player);
            }
            catch(Exception ex)
            {
                return ManagerResponse<Player>.Failure($"Error retrieving player with Id: {playerId}");
            }
        }

        public async Task<ManagerResponse<bool>> DeleteAsync(Player player)
        {
            try
            {
                unitOfWork.Players.Remove(player);

                await unitOfWork.SaveChangesAsync();

                return ManagerResponse<bool>.Success();
            }
            catch(Exception ex)
            {
                return ManagerResponse<bool>.Failure($"Error deleting player with Id: {player.Name}");
            }
        }

        public async Task<ManagerResponse<List<Player>>> Get()
        {
            try
            {
                var players = await unitOfWork.Players.Get()
                    .Include(x => x.PlayerSkills)
                    .ToListAsync();

                return ManagerResponse<List<Player>>.Success(players);
            }
            catch(Exception ex)
            {
                return ManagerResponse<List<Player>>.Failure("Error retreiving Players");
            }
        }

        public async Task<ManagerResponse<long>> UpdateAsync(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            try
            {
                var result = unitOfWork.Players.Update(player);

                await unitOfWork.SaveChangesAsync();

                return ManagerResponse<long>.Success(result.Id);
            }
            catch (Exception ex)
            {
                return ManagerResponse<long>.Failure($"Error updating player info for: {player.Name}");
            }
        }
    }
}
