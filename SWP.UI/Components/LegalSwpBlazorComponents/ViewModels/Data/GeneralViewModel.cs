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

        public const string Client = "Klient";
        public const string Case = "Sprawa";
        public const string Reminder = "Przypomnienie";
        public const string Deadline = "Termin";
        public const string Created = "Stworzone";
        public const string CreatedBy = "Użytkownik";
        public const string Updated = "Ostania Zmiana";
        public const string UpdatedBy = "Użytkownik";

        public const string Day = "Dzień";
        public const string Week = "Tydzień";
        public const string Month = "Miesiąc";

        public const string DateFormant = "dd/MM/yy HH:mm";
        public const int NotificationDuration = 4000;

        public DialogOptions DefaultDialogOptions => new DialogOptions() { Width = "500px", Height = "530px", Left = "calc(50% - 500px)", Top = "calc(50% - 265px)" };

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
