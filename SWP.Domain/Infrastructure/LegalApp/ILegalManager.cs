using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure.LegalApp
{
    public interface ILegalManager
    {
        #region Clients

        Client GetClient(int clientId);
        Client GetClientWithoutCases(int clientId);
        List<Client> GetClientsWithoutCases(string profile, bool active = true);
        Task<Client> CreateClient(Client client);
        Task<Client> UpdateClient(Client client);
        Task<int> DeleteClient(int clientId);
        Task<int> DeleteProfileClients(string profile);
        string GetClientName(int id);

        #endregion

        #region Jobs

        Task<ClientJob> CreateClientJob(int clientId, ClientJob job);
        Task<int> DeleteClientJob(int id);
        Task<ClientJob> UpdateClientJob(ClientJob job);
        ClientJob GetClientJob(int id);
        List<ClientJob> GetClientJobs(int clientId);

        #endregion

        #region Cases

        List<Case> GetCasesForClient(int clientId);
        Case GetCase(int id);
        List<Case> GetArchivedCases(int clientId);
        Case GetCaseWithoutData(int id);
        string GetCaseParentName(int id);
        string GetCaseName(int id);
        string GetClientNameForCase(int id);

        Task<Case> CreateCase(int clientId, string profile, Case c);
        Task<Case> UpdateCase(Case c);
        Task<int> DeleteCase(int id);
        int CountCases(int clientId);

        #endregion

        #region Reminders

        Reminder GetReminder(int id);
        List<Reminder> GetReminders(string profile);
        List<Reminder> GetRemindersForClient(int clientId);

        List<Reminder> GetUpcomingReminders(int clientId, DateTime startDate);
        List<Reminder> GetUpcomingReminders(string profile, DateTime startDate);

        Task<Reminder> CreateReminder(int caseId, Reminder reminder);
        Task<Reminder> UpdateReminder(Reminder reminder);
        Task<int> DeleteReminder(int id);

        Task<int> CountReminders();

        #endregion

        #region Notes

        Note GetNote(int id);
        List<Note> GetNotesForCase(int caseId);

        Task<Note> CreateNote(int caseId, Note note);
        Task<Note> UpdateNote(Note note);
        Task<int> DeleteNote(int id);

        #endregion

        #region Statistics

        int CountClients();
        int CountCasesPerClient(int clientId);
        int CountJobsPerClient(int clientId);

        IEnumerable<int> GetClientsIds(string profile);
        IEnumerable<int> GetClientCasesIds(int clientId);

        int CountRemindersPerCase(int caseId);
        int CountDeadlineRemindersPerCase(int caseId);
        int CountNotesPerCase(int caseId);

        #endregion

        #region Cash Movements

        CashMovement GetCashMovement(int id);
        List<CashMovement> GetCashMovementsForClient(int clientId);
        Task<CashMovement> CreateCashMovement(int clientId, string profile, CashMovement cashMovement);
        Task<CashMovement> UpdateCashMovement(CashMovement cashMovement);
        Task<int> DeleteCashMovement(int id);

        #endregion

        #region Cash Movements

        TimeRecord GetTimeRecord(int id);
        List<TimeRecord> GetTimeRecords(int clientId);
        Task<TimeRecord> CreateTimeRecord(int clientId, string profile, TimeRecord cashMovement);
        Task<TimeRecord> UpdateTimeRecord(TimeRecord cashMovement);
        Task<int> DeleteTimeRecord(int id);

        #endregion

        #region Archive

        int CountArchivedCases();
        int CountArchivedClients();

        Task<int> ArchivizeClient(int clientId, string user);
        Task<int> ArchivizeCase(int caseId, string user);
        Task<ClientJob> ArchivizeClientJob(int jobId, string user);
        Task<int> ArchivizeNote(int noteId, string user);

        Task<int> RecoverClient(int clientId, string user);
        Task<int> RecoverCase(int caseId, string user);
        Task<ClientJob> RecoverClientJob(int jobId, string user);
        Note RecoverNote(int noteId, string user);

        #endregion

        #region Contact People

        CaseContactPerson GetCaseContactPerson(int id);
        ClientContactPerson GetClientContactPerson(int id);
        List<ClientContactPerson> GetContactPeopleForClient(int clientId);
        List<CaseContactPerson> GetContactPeopleForCase(int caseId);
        Task<ClientContactPerson> CreateClientContactPerson(int clientId, ClientContactPerson contactPerson);
        Task<CaseContactPerson> CreateCaseContactPerson(int caseId, CaseContactPerson contactPerson);
        Task<ClientContactPerson> UpdateClientContactPerson(ClientContactPerson contactPerson);
        Task<CaseContactPerson> UpdateCaseContactPerson(CaseContactPerson contactPerson);
        Task<int> DeleteClientContactPerson(int id);
        Task<int> DeleteCaseContactPerson(int id);

        #endregion
    }
}
