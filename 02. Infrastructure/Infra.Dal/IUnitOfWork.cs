using System.Threading.Tasks;

namespace Infra.Dal
{
    public interface IUnitOfWork
    {
        int Commit();
        Task<int> CommitAsync();
    }
}
