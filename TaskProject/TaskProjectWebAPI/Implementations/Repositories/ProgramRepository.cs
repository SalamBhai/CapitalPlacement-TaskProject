using TaskConsole.Models;
using TaskProjectWebAPI.Context;
using TaskProjectWebAPI.Interfaces.Repositories;

namespace TaskProjectWebAPI.Implementations.Repositories
{
    public class ProgramRepository : GenericRepository<TaskConsole.Models.Program>, IProgramRepository
    {
        public ProgramRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
