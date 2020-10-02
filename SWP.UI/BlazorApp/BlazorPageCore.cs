using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp
{
    public abstract class BlazorPageCore
    {
        protected readonly IServiceProvider serviceProvider;

        public BlazorPageCore(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

        protected void LogException(string userId, string message, string stack)
        { 
        
        
        
        
        }
    }
}
