﻿using SWP.Domain.Models.SWPLegal;
using System;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class TimeRecordViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TimeSpan RecordedTime { get; set; }
        public int ClientId { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public static implicit operator TimeRecordViewModel(TimeRecord input) =>
            new TimeRecordViewModel
            {
                Id = input.Id,
                Description = input.Description,
                RecordedTime = input.RecordedTime,
                Name = input.Name,
                ClientId = input.ClientId,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}