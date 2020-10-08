﻿using SWP.Domain.Models.SWPLegal;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class TimeRecordViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public int RecordedHours { get; set; } = 0;
        public int RecordedMinutes { get; set; } = 0;

        public int ClientId { get; set; }

        public DateTime EventDate { get; set; }

        public static implicit operator TimeRecordViewModel(TimeRecord input) =>
            new TimeRecordViewModel
            {
                Id = input.Id,
                Description = input.Description,
                RecordedHours = input.Hours,
                RecordedMinutes = input.Minutes,
                EventDate = input.EventDate,
                Name = input.Name,
                ClientId = input.ClientId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}
