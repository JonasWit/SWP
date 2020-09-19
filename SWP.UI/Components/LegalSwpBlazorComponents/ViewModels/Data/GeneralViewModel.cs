using System.Collections.Generic;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    [UITransientService]
    public class GeneralViewModel
    {
        public string DeadlineColor => "#DC143C";

        public class Priority
        {
            public int Number { get; set; }
        }

        //todo: dodac consty na polskie nazwy kolumn

        public class PriorityColor
        {
            public int Number { get; set; }
            public string BackgroundColor { get; set; }
            public string TextColor { get; set; }
        }

        public List<Priority> Priorities =>
            new List<Priority>
            {
                new Priority { Number = 1 },
                new Priority { Number = 2 },
                new Priority { Number = 3 },
                new Priority { Number = 4 },
                new Priority { Number = 5 },
            };

        public List<PriorityColor> PrioritiesColors =>
            new List<PriorityColor>
            {
                new PriorityColor { Number = 1, BackgroundColor = "#FF6600", TextColor = "#000000" },
                new PriorityColor { Number = 2, BackgroundColor = "#FFFF00", TextColor = "#000000" },
                new PriorityColor { Number = 3, BackgroundColor = "#CCFF00", TextColor = "#000000" },
                new PriorityColor { Number = 4, BackgroundColor = "#00CC00", TextColor = "#000000" },
                new PriorityColor { Number = 5, BackgroundColor = "#003300", TextColor = "#FFFFFF" },
            };
    }
}
