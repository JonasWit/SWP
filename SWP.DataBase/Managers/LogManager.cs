using SWP.Domain.Infrastructure;

namespace SWP.DataBase.Managers
{
    public class LogManager : DataManagerBase, ILogManager
    {
        public LogManager(AppContext context) : base(context)
        {
        }

    }
}
