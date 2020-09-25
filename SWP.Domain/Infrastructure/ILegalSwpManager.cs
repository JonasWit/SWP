using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Domain.Infrastructure
{
    public interface ILegalSwpManager
    {
        #region Clients

        TResult GetClient<TResult>(int id, string profile, Func<Client, TResult> selector);
        TResult GetClientWithoutCases<TResult>(int id, string profile, Func<Client, TResult> selector);
        List<TResult> GetClients<TResult>(string profile, Func<Client, TResult> selector);
        List<Client> GetClientsWithoutCases(string profile);

        Task<Client> CreateClient(Client client);
        Task<Client> UpdateClient(Client client);
        Task<int> DeleteClient(int id);
        Task<int> DeleteProfileClients(string profile);

        #endregion

        #region Jobs

        Task<ClientJob> CreateClientJob(int clientId, string profile, ClientJob job);
        Task<int> DeleteClientJob(int id);
        Task<ClientJob> UpdateClientJob(ClientJob job);
        TResult GetClientJob<TResult>(int id, Func<ClientJob, TResult> selector);

        #endregion

        #region Cases
        TResult GetCase<TResult>(int id, Func<Case, TResult> selector);
        TResult GetCaseWithoutData<TResult>(int id, Func<Case, TResult> selector);
        TResult GetCases<TResult>(string profile, Func<Case, TResult> selector);
        string GetCaseParentName(int id);
        string GetCaseName(int id);

        Task<Case> CreateCase(int clientId, string profile, Case c);
        Task<Case> UpdateCase(Case c);
        Task<int> DeleteCase(int id);
        int CountCases(int clientId);

        #endregion

        #region Reminders

        TResult GetReminder<TResult>(int id, Func<Reminder, TResult> selector);
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

    }
}
