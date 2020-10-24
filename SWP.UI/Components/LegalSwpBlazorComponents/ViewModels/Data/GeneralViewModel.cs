using iTextSharp.text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Radzen;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    [UITransientService]
    public class GeneralViewModel
    {
        public string DeadlineColor => "#DC143C";
        public const int NotificationDuration = 4000;

        public string FormInputBoxWidth => @"width: 100%; font-size: 12px;";

        public string TableFontSize => @"font-size: 12px;";

        public string ToolTipStyle => @"font-size: 12px; background-color: #000;";

        public string GeneralButton => @"font-size: 12px;";




        public DialogOptions DefaultDialogOptions => new DialogOptions() { Width = "500px", Height = "530px", Left = "calc(50% - 500px)", Top = "calc(50% - 265px)" };

        public string ReminderDispalyDate(DateTime dateTime) => (dateTime.Hour == 0 && dateTime.Minute == 0) ? dateTime.ToString("dd/MM/yyyy") : dateTime.ToString("dd/MM/yyyy HH:mm");

        public class Priority
        {
            public int Number { get; set; }
        }

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
