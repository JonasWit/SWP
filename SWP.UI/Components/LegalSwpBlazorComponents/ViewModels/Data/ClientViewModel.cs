using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SWP.UI.Components.LegalSwpBlazorComponents.ViewModels.Data
{
    public class ClientViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public bool Active { get; set; }
        public string ProfileClaim { get; set; }
        public string Address { get; set; }

        public ClientJobViewModel SelectedJob { get; set; }
        public CaseViewModel SelectedCase { get; set; }
        public TimeRecordViewModel SelectedTimeRecord { get; set; }
        public CashMovementViewModel SelectedCashMovement { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public List<CaseViewModel> Cases { get; set; }
        public List<CaseViewModel> ArchivedCases { get; set; }

        public List<ClientJobViewModel> Jobs { get; set; }
        public List<ClientJobViewModel> ArchivedJobs { get; set; }

        public List<TimeRecordViewModel> TimeRecords { get; set; }
        public List<CashMovementViewModel> CashMovements { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }

        public string GetTimeSpent()
        {
            if (TimeRecords.Count == 0) return "Brak zarejestrowanego czasu.";

            var spentTime = new TimeSpan(TimeRecords.Sum(x => x.RecordedHours), TimeRecords.Sum(x => x.RecordedMinutes), 0);
            var result = $"Poświęcony czas: {spentTime.Days} dn. {spentTime.Hours} godz. {spentTime.Minutes} min.";

            return result;
        }

        public static implicit operator ClientViewModel(Client input) =>
            new ClientViewModel
            { 
                Id = input.Id,
                Name = input.Name,
                Active = input.Active,
                ProfileClaim = input.ProfileClaim,
                Address = input.Address,
                Email = input.Email,
                PhoneNumber = input.PhoneNumber,
                Cases = input.Cases == null ? new List<CaseViewModel>() : input.Cases.Where(x => x.Active).Select(x => (CaseViewModel)x).ToList(),
                ArchivedCases = input.Cases == null ? new List<CaseViewModel>() : input.Cases.Where(x => !x.Active).Select(x => (CaseViewModel)x).ToList(),
                Jobs = input.Jobs == null ? new List<ClientJobViewModel>() : input.Jobs.Where(x => x.Active).Select(x => (ClientJobViewModel)x).ToList(),
                ArchivedJobs = input.Jobs == null ? new List<ClientJobViewModel>() : input.Jobs.Where(x => !x.Active).Select(x => (ClientJobViewModel)x).ToList(),
                TimeRecords = input.TimeRecords == null ? new List<TimeRecordViewModel>() : input.TimeRecords.Select(x => (TimeRecordViewModel)x).ToList(),
                CashMovements = input.CashMovements == null ? new List<CashMovementViewModel>() : input.CashMovements.Select(x => (CashMovementViewModel)x).ToList(),
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy
            };
    }
}
