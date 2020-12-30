using SWP.Domain.Infrastructure.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp
{
    public abstract class LegalActionsBase
    {
        protected readonly ILegalManager _legalManager;
        public LegalActionsBase(ILegalManager legalManager) => _legalManager = legalManager;
    }
}
