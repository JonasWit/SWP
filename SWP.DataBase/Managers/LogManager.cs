using SWP.Domain.Infrastructure;
using SWP.Domain.Models.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class LogManager : DataManagerBase, ILogManager
    {
        public LogManager(ApplicationDbContext context) : base(context)
        {
        }

    }
}
