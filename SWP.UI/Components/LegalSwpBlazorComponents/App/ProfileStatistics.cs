using SWP.Application.LegalSwp.Cases;
using SWP.Application.LegalSwp.Clients;
using System.Collections.Generic;

namespace SWP.UI.Components.LegalSwpBlazorComponents.App
{
    [UITransientService]
    public class ProfileStatistics
    {
        private readonly GetClients getClients;
        private readonly GetCases getCases;

        public ProfileStatistics(GetClients getClients, GetCases getCases)
        {
            this.getClients = getClients;
            this.getCases = getCases;
        }

        public class DataItem
        {
            public string Quarter { get; set; }
            public double Revenue { get; set; }
        }

        public Response GetStatistics(string profile)
        {
            var Clients = getClients.GetClientsWithoutData(profile);
            var response = new Response();

            foreach (var Client in Clients)
            {
                var ClientStatistic = new Response.ClientStatistic
                {
                    Name = Client.Name
                };

                var ClientCases = getClients.GetClientCasesIds(Client.Id);

                ClientStatistic.Jobs = getClients.CountJobsPerClient(Client.Id) ?? 0;

                if (ClientCases != null)
                {
                    foreach (var cs in ClientCases)
                    {
                        ClientStatistic.Cases.Add(new Response.CaseStatistic
                        {
                            Id = cs,
                            Deadlines = getCases.CountDeadlineRemindersPerCase(cs),
                            Reminders = getCases.CountRemindersPerCase(cs),
                            Notes = getCases.CountNotesPerCase(cs)
                        });
                    }
                }

                response.Clients.Add(ClientStatistic);
            }

            return response;
        }

        public class Response
        {
            public string ProfileName { get; set; }

            public List<ClientStatistic> Clients { get; set; } = new List<ClientStatistic>();

            public class ClientStatistic
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int Jobs { get; set; }
                public List<CaseStatistic> Cases { get; set; } = new List<CaseStatistic>();
            }

            public class CaseStatistic
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public int Reminders { get; set; }
                public int Deadlines { get; set; }
                public int Notes { get; set; }
            }
        }
    }
}
