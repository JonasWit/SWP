using SWP.Domain.Enums;
using SWP.Domain.Models.Portal.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.Components.PortalBlazorComponents.Requests.ViewModels
{
    public class RequestViewModel
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string RequestorId { get; set; }
        public RequestStatus Status { get; set; }
        public RequestReason Reason { get; set; }
        public ApplicationType Application { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int RelatedUsers { get; set; }
        public string DisplaySubject { get; set; }
        public string DisplayStatus { get; set; }
        public string DisplayApplication => Application.ToString();

        public List<RequestMessageViewModel> Messages { get; set; } = new List<RequestMessageViewModel>();

        public static Dictionary<int, string> RequestReasonsDisplay()
        {
            var result = new Dictionary<int, string>();
            var enumValues = Enum.GetValues(typeof(RequestReason)).Cast<RequestReason>();

            foreach (var item in enumValues)
            {
                switch (item)
                {
                    case RequestReason.Query:
                        result.Add((int)RequestReason.Query, "Zapytanie Biznesowe");
                        break;
                    case RequestReason.PurchaseLicense:
                        result.Add((int)RequestReason.PurchaseLicense, "Zakup Licencji");
                        break;
                    case RequestReason.ModifyLicense:
                        result.Add((int)RequestReason.ModifyLicense, "Zmiana / Modyfikacja Licencji");
                        break;
                    case RequestReason.ExtendLicense:
                        result.Add((int)RequestReason.ExtendLicense, "Przedłużenie Licencji");
                        break;
                    case RequestReason.RequestDemo:
                        result.Add((int)RequestReason.RequestDemo, "Prośba o dostęp testowy");
                        break;
                    case RequestReason.TechnicalIssue:
                        result.Add((int)RequestReason.TechnicalIssue, "Pomoc Techniczna");
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public static Dictionary<int, string> RequestReasonsTemplateMessageDisplay()
        {
            var result = new Dictionary<int, string>();
            var enumValues = Enum.GetValues(typeof(RequestReason)).Cast<RequestReason>();

            foreach (var item in enumValues)
            {
                switch (item)
                {
                    case RequestReason.Query:
                        result.Add((int)RequestReason.Query, "Zapytanie Biznesowe.");
                        break;
                    case RequestReason.PurchaseLicense:
                        result.Add((int)RequestReason.PurchaseLicense, "Proszę o przyznanie licencji.");
                        break;
                    case RequestReason.ModifyLicense:
                        result.Add((int)RequestReason.ModifyLicense, "Proszę z zmianę ustawień mojej licencji w następujący sposób.");
                        break;
                    case RequestReason.ExtendLicense:
                        result.Add((int)RequestReason.ExtendLicense, "Proszę o przedłużenie mojej licencji.");
                        break;
                    case RequestReason.RequestDemo:
                        result.Add((int)RequestReason.RequestDemo, "Prośba o dostęp testowy.");
                        break;
                    case RequestReason.TechnicalIssue:
                        result.Add((int)RequestReason.TechnicalIssue, "Proszę o pomoc techniczną.");
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public static Dictionary<int, string> RequestStatusesDisplay()
        {
            var result = new Dictionary<int, string>();
            var enumValues = Enum.GetValues(typeof(RequestStatus)).Cast<RequestStatus>();

            foreach (var item in enumValues)
            {
                switch (item)
                {
                    case RequestStatus.WaitingForAnswer:
                        result.Add((int)RequestStatus.WaitingForAnswer, "Czeka na odpowiedź");
                        break;
                    case RequestStatus.Answered:
                        result.Add((int)RequestStatus.Answered, "Udzielono odpowiedzi");
                        break;
                    case RequestStatus.Solved:
                        result.Add((int)RequestStatus.Solved, "Zamknięty");
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public static Dictionary<int, string> ApplicationssDisplay()
        {
            var result = new Dictionary<int, string>();
            var enumValues = Enum.GetValues(typeof(ApplicationType)).Cast<ApplicationType>();

            foreach (var item in enumValues)
            {
                switch (item)
                {
                    case ApplicationType.LegalApplication:
                        result.Add((int)ApplicationType.LegalApplication, "Twoja Kancelaria");
                        break;
                    case ApplicationType.NoApp:
                        result.Add((int)ApplicationType.NoApp, "Inne / Portal / Zapytanie Ogólne");
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public static implicit operator RequestViewModel(ClientRequest input)
        {
            var result = new RequestViewModel
            {
                Id = input.Id,
                Created = input.Created,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                CreatedBy = input.CreatedBy,
                RequestorId = input.RequestorId,
                Status = (RequestStatus)input.Status,
                Reason = (RequestReason)input.Reason,
                Application = (ApplicationType)input.Application,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                RelatedUsers = input.RelatedUsers,
                Messages = input.Messages == null ? new List<RequestMessageViewModel>() : input.Messages.Select(x => (RequestMessageViewModel)x).ToList().OrderByDescending(x => x.Updated).ToList(),
                DisplaySubject = RequestReasonsDisplay()[input.Reason],
                DisplayStatus = RequestStatusesDisplay()[input.Status],
            };

            return result;
        }

        public static implicit operator ClientRequest(RequestViewModel input)
        {
            var result = new ClientRequest
            {
                Id = input.Id,
                Updated = input.Updated,
                UpdatedBy = input.UpdatedBy,
                Status = (int)input.Status,
            };

            return result;
        }
    }
}
