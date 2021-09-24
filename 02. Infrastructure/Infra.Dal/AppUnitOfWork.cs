using System.Threading.Tasks;

namespace Infra.Dal
{
    public sealed class AppUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;

        public AppUnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Commit()
        {
            return _dbContext.SaveChanges();
        }

        public Task<int> CommitAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
