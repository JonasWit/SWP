using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public abstract class DataManagerBase
    {
        protected readonly AppContext _context;
        protected DataManagerBase(AppContext context) => _context = context;
    }
}
