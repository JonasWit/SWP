using SWP.Application.LegalSwp.Clients;

namespace SWP.UI.BlazorApp.LegalApp.Stores.Clients.Actions
{
    public class SubmitNewClientAction : IAction
    {
        public const string SubmitNewClient = "SUBMIT_NEW_CLIENT";
        public string Name => SubmitNewClient;

        public CreateClient.Request Arg { get; set; }
    }
}
