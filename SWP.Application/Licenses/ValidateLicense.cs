using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWP.Application.Licenses
{
    [TransientService]
    public class ValidateLicense
    {
        private readonly ILicensesManager licensesManager;

        public ValidateLicense(ILicensesManager licensesManager) => this.licensesManager = licensesManager;


        public class Request
        { 
        
        }

        public class Response
        { 
        
        
        }

    }
}
