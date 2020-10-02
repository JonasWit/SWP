using SWP.Application.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SWP.UI.BlazorApp
{
    public abstract class BlazorPageCore
    {
        protected readonly IServiceProvider serviceProvider;

        private CreateLogEntry CreateLogEntry => serviceProvider.GetService<CreateLogEntry>();

        public BlazorPageCore(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        protected void LogException(string userId, string message, string stack)
        { 
 
        
        }

        protected void GetLogRecords()
        {


        }

        protected void GetLogRecord(int id)
        {


        }

        protected void DeleteLogRecord(int id)
        {


        }


    }
}
