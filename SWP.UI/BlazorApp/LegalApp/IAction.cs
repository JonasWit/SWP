using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.LegalApp
{
    public interface IAction
    {
        public string Name { get; }
    }

    public interface IAction<Task>
    {
        public string Name { get; }
    }
}
