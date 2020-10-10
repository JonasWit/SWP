using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp
{
    public abstract class BlazorPageBase : BlazorPageCore
    {
        public MonthFilterRecord SelectedMonth { get; set; }
        public List<MonthFilterRecord> MonthsFilterData { get; set; } = new List<MonthFilterRecord>();

        public class MonthFilterRecord
        {
            public int Id { get; set; }
            public string DisplayText => $"{Month}-{Year}";
            public int Month { get; set; }
            public int Year { get; set; }
        }

        public abstract Task Initialize(BlazorAppBase app);

        public BlazorPageBase(IServiceProvider serviceProvider) : base (serviceProvider) { }
    }
}
