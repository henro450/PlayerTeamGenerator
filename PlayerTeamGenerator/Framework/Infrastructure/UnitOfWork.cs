using PlayerTeamGenerator.Entities;
using PlayerTeamGenerator.Helpers;
using static PlayerTeamGenerator.Framework.Application.IRepository;

namespace PlayerTeamGenerator.Framework.Infrastructure
{
    public class UnitOfWork
    {
        private readonly DataContext context;

        public UnitOfWork()
        {

        }

        public UnitOfWork(DataContext context)
        {
            this.context = context;
        }

        public IRepository<Player> Players => new Repository<Player>(context);
        public IRepository<PlayerSkill> PlayerSkills => new Repository<PlayerSkill>(context);

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Could not commit changes to the database", ex);
            }
        }
    }
}
