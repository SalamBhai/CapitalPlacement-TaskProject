using TaskConsole.Models;
using TaskProjectWebAPI.Context;
using TaskProjectWebAPI.Interfaces.Repositories;

namespace TaskProjectWebAPI.Implementations.Repositories
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
