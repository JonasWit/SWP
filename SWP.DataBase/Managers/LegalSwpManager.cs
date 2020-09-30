using Microsoft.EntityFrameworkCore;
using SWP.Domain.Infrastructure;
using SWP.Domain.Models.SWPLegal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class LegalSwpManager : ILegalSwpManager
    {
        private readonly ApplicationDbContext context;
        public LegalSwpManager(ApplicationDbContext context) => this.context = context;

        #region Client

        public Client GetClient(int clientId) =>
            context.Clients
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Include(x => x.CashMovements)
                .Include(x => x.TimeRecords)
                .Where(x => x.Id == clientId)
                .FirstOrDefault();

        public Client GetClientWithoutCases(int clientId) =>
            context.Clients
                .Include(x => x.Jobs)
                .Where(x => x.Id == clientId)
                .FirstOrDefault();

        public List<Client> GetClientsWithoutCases(string profile, bool active = true) =>
            context.Clients
                .Include(x => x.Jobs)
                .Where(x => x.ProfileClaim == profile && active ? x.Active : !x.Active)
                .ToList();

        public async Task<Client> CreateClient(Client client)
        {
            context.Clients.Add(client);
            await context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClient(Client client)
        {
            context.Clients.Update(client);
            await context.SaveChangesAsync();
            return client;
        }

        public Task<int> DeleteClient(int id)
        {
            context.Clients.Remove(context.Clients.FirstOrDefault(x => x.Id == id));
            return context.SaveChangesAsync();
        }

        public Task<int> DeleteProfileClients(string profile)
        {
            var clients = context.Clients
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.ProfileClaim == profile)
                .Select(x => x.Id)
                .ToList();

            context.Clients.RemoveRange(context.Clients.Where(x => clients.Contains(x.Id)));
            return context.SaveChangesAsync();
        }

        public int CountClients() => context.Clients.AsNoTracking().Count(x => x.Active);

        #endregion

        #region Case

        public Case GetCase(int id) =>
            context.Cases
                .Include(x => x.Notes)
                .Include(x => x.Reminders)
                .Where(x => x.Id == id)
                .FirstOrDefault();

        public Case GetCaseWithoutData(int id) =>
            context.Cases
                .Where(x => x.Id == id)
                .FirstOrDefault();

        public Task<int> DeleteCase(int id)
        {
            var caseEntity = context.Cases.FirstOrDefault(x => x.Id == id);
            context.Cases.Remove(caseEntity);
            return context.SaveChangesAsync();
        }

        public async Task<Case> CreateCase(int clientId, string profile, Case c)
        {
            var ClientEntity = context.Clients
                .Include(x => x.Cases)
                .FirstOrDefault(x => x.ProfileClaim == profile && x.Id == clientId);

            ClientEntity.Cases.Add(c);
            await context.SaveChangesAsync();
            return c;
        }

        public async Task<Case> UpdateCase(Case c)
        {
            context.Cases.Update(c);
            await context.SaveChangesAsync();
            return c;
        }

        public int CountCases(int clientId) => context.Cases.Count(x => x.ClientId == clientId);

        public string GetCaseParentName(int id) => context.Clients.Where(x => x.Cases.Any(y => y.Id == id)).Select(x => x.Name).FirstOrDefault();

        public string GetCaseName(int id) => context.Cases.Where(x => x.Id == id).Select(y => y.Name).FirstOrDefault();

        #endregion

        #region Reminder

        public Reminder GetReminder(int id) =>
            context.Reminders
                .Where(x => x.Id == id)
                .FirstOrDefault();

        public List<Reminder> GetReminders(string profile) =>
            context.Clients
                .Where(x => x.ProfileClaim == profile)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public List<Reminder> GetRemindersForClient(int clientId) =>
            context.Clients
                .Where(x => x.Id == clientId)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public async Task<Reminder> CreateReminder(int caseId, Reminder reminder)
        {
            var caseEntity = context.Cases
                .Include(x => x.Reminders)
                .FirstOrDefault(x => x.Id == caseId);

            caseEntity.Reminders.Add(reminder);
            await context.SaveChangesAsync();
            return reminder;
        }

        public async Task<Reminder> UpdateReminder(Reminder reminder)
        {
            context.Reminders.Update(reminder);
            await context.SaveChangesAsync();
            return reminder;
        }

        public Task<int> DeleteReminder(int id)
        {
            var reminder = context.Reminders.FirstOrDefault(x => x.Id == id);
            context.Reminders.Remove(reminder);
            return context.SaveChangesAsync();
        }

        public Task<int> CountReminders()
        {
            throw new NotImplementedException();
        }

        public List<Reminder> GetUpcomingReminders(int clientId, DateTime startDate) => 
            context.Clients
                .Where(x => x.Id == clientId)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .Where(x => x.Start <= startDate)
                .ToList();

        public List<Reminder> GetUpcomingReminders(string profile, DateTime startDate) =>
            context.Clients
                .Where(x => x.ProfileClaim == profile)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .Where(x => x.Start <= startDate)
                .ToList();

        #endregion

        #region Note

        public Note GetNote(int id) => context.Notes.AsNoTracking().FirstOrDefault(x => x.Id == id);

        public List<Note> GetNotesForCase(int caseId) =>
            context.Notes
                .Where(x => x.CaseId == caseId)
                .ToList();

        public async Task<Note> CreateNote(int caseId, Note note)
        {
            var caseEntity = context.Cases
                .Include(x => x.Notes)
                .FirstOrDefault(x => x.Id == caseId);

            caseEntity.Notes.Add(note);
            await context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateNote(Note note)
        {
            context.Notes.Update(note);
            await context.SaveChangesAsync();
            return note;
        }

        public Task<int> DeleteNote(int id)
        {
            var note = context.Notes.FirstOrDefault(x => x.Id == id);
            context.Notes.Remove(note);
            return context.SaveChangesAsync();
        }

        #endregion

        #region Job

        public async Task<ClientJob> CreateClientJob(int clientId, ClientJob job)
        {
            var client = context.Clients
                .Include(x => x.Jobs)
                .FirstOrDefault(x => x.Id == clientId);

            client.Jobs.Add(job);
            await context.SaveChangesAsync();
            return job;
        }

        public Task<int> DeleteClientJob(int id)
        {
            var job = context.ClientJobs.FirstOrDefault(x => x.Id == id);
            context.ClientJobs.Remove(job);
            return context.SaveChangesAsync();
        }

        public ClientJob GetClientJob(int id) =>
            context.ClientJobs
                .Where(x => x.Id == id)
                .FirstOrDefault();

        public async Task<ClientJob> UpdateClientJob(ClientJob job)
        {
            context.ClientJobs.Update(job);
            await context.SaveChangesAsync();
            return job;
        }

        #endregion

        #region Cash Movements

        public CashMovement GetCashMovement(int id) => context.CashMovements.AsNoTracking().FirstOrDefault(x => x.Id == id);

        public List<CashMovement> GetCashMovementsForClient(int clientId) =>
            context.CashMovements
                .Where(x => x.ClientId ==clientId)
                .ToList();

        public async Task<CashMovement> CreateCashMovement(int clientId, string profile, CashMovement cashMovement)
        {
            var client = context.Clients
                .Include(x => x.CashMovements)
                .FirstOrDefault(x => x.Id == clientId && x.ProfileClaim == profile);

            client.CashMovements.Add(cashMovement);
            await context.SaveChangesAsync();
            return cashMovement;
        }

        public async Task<CashMovement> UpdateCashMovement(CashMovement cashMovement)
        {
            context.CashMovements.Update(cashMovement);
            await context.SaveChangesAsync();
            return cashMovement;
        }

        public Task<int> DeleteCashMovement(int id)
        {
            var cm = context.CashMovements.FirstOrDefault(x => x.Id == id);
            context.CashMovements.Remove(cm);
            return context.SaveChangesAsync();
        }

        #endregion

        #region Time Records

        public TimeRecord GetTimeRecord(int id) => context.TimeRecords.FirstOrDefault(x => x.Id == id);

        public List<TimeRecord> GetTimeRecords(int clientId) =>
            context.TimeRecords
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .ToList();

        public async Task<TimeRecord> CreateTimeRecord(int clientId, string profile, TimeRecord timeRecord)
        {
            var client = context.Clients
                .Include(x => x.TimeRecords)
                .FirstOrDefault(x => x.Id == clientId && x.ProfileClaim == profile);

            client.TimeRecords.Add(timeRecord);
            await context.SaveChangesAsync();
            return timeRecord;
        }

        public async Task<TimeRecord> UpdateTimeRecord(TimeRecord timeRecord)
        {
            context.TimeRecords.Update(timeRecord);
            await context.SaveChangesAsync();
            return timeRecord;
        }

        public Task<int> DeleteTimeRecord(int id)
        {
            var tr = context.TimeRecords.FirstOrDefault(x => x.Id == id);
            context.TimeRecords.Remove(tr);
            return context.SaveChangesAsync();
        }

        #endregion

        #region Statistics

        public int CountCasesPerClient(int clientId) => 
            context.Cases.AsNoTracking().Count(x => x.ClientId == clientId);

        public int CountJobsPerClient(int clientId) => 
            context.ClientJobs.AsNoTracking().Count(x => x.ClientId == clientId);

        public int CountRemindersPerCase(int caseId) => 
            context.Reminders.AsNoTracking()
                .Count(y => y.CaseId == caseId && y.End >= DateTime.Now);

        public int CountDeadlineRemindersPerCase(int caseId) => 
            context.Reminders.AsNoTracking()
                .Count(y => y.CaseId == caseId && y.End >= DateTime.Now && y.IsDeadline);

        public int CountNotesPerCase(int caseId) => 
            context.Notes.AsNoTracking().Count(x => x.CaseId == caseId && x.Active);

        public IEnumerable<int> GetClientCasesIds(int clientId) =>
            context.Clients
                .AsNoTracking()
                .Include(x => x.Cases)
                .FirstOrDefault(x => x.Id == clientId && x.Active).Cases
                .Select(y => y.Id)
                .ToList();

        public IEnumerable<int> GetClientsIds(string profile) =>
            context.Clients
                .AsNoTracking()
                .Where(x => x.ProfileClaim == profile && x.Active)
                .Select(y => y.Id)
                .ToList();

        #endregion

        #region Archive

        public int CountArchivedCases() => context.Clients
            .Include(x => x.Cases)
            .SelectMany(x => x.Cases)
            .Where(x => !x.Active)
            .Count();

        public int CountArchivedClients() => context.Clients.Count(x => !x.Active);

        public Task<int> ArchivizeClient(int clientId)
        {
            var client = context.Clients
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.Id == clientId)
                .FirstOrDefault();

            client.Active = false;

            foreach (var c in client.Cases)
            {
                c.Active = false;
                foreach (var n in c.Notes)
                {
                    n.Active = false;
                }
            }

            foreach (var j in client.Jobs)
            {
                j.Active = false;
            }

            context.Clients.Update(client);
            return context.SaveChangesAsync();
        }

        public Task<int> ArchivizeCase(int caseId)
        {
            var c = context.Cases.FirstOrDefault(x => x.Id == caseId);
            c.Active = false;

            foreach (var n in c.Notes)
            {
                n.Active = false;
            }

            context.Cases.Update(c);
            return context.SaveChangesAsync();
        }

        public Task<int> ArchiveClientJob(int jobId)
        {
            var job = context.ClientJobs.FirstOrDefault(x => x.Id == jobId);
            job.Active = false;

            context.ClientJobs.Update(job);
            return context.SaveChangesAsync();
        }

        public Task<int> ArchiveNote(int noteId)
        {
            var note = context.Notes.FirstOrDefault(x => x.Id == noteId);
            note.Active = false;

            context.Notes.Update(note);
            return context.SaveChangesAsync();
        }

        public List<Case> GetArchivedCases() => context.Cases.Where(x => !x.Active).ToList();

        public Client RecoverClient(int clientId)
        {
            throw new NotImplementedException();
        }

        public Case RecoverCase(int caseId)
        {
            throw new NotImplementedException();
        }

        public ClientJob RecoverClientJob(int jobId)
        {
            throw new NotImplementedException();
        }

        public Note RecoverNote(int noteId)
        {
            throw new NotImplementedException();
        }


        #endregion
    }

}
