using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.ViewModels.LegalApp.Statistics
{
    public class DateDataItem
    {
        public double TimeNumber => Time != null ? (double)Time.Days * 24 + Time.Hours + (double)Time.Minutes / 100 : 0;
        public int TotalHours => Time != null ? Time.Days * 24 + Time.Hours : 0;
        public TimeSpan Time { get; set; }
        public DateTime Date { get; set; }
        public double Number { get; set; } = 0;
        public double Expenses { get; set; } = 0;
        public double ProductivityRatio => TotalHours != 0 ? Math.Round(Number / TotalHours, 2) : 0;
    }
}
