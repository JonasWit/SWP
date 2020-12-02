using Microsoft.EntityFrameworkCore;
using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.DataBase.Managers
{
    public class LegalManager : DataManagerBase, ILegalManager
    {
        public LegalManager(ApplicationDbContext context) : base(context)
        {
        }

        #region Client

        public Client GetClient(int clientId) =>
            _context.Clients
                .Where(x => x.Id == clientId)
                .Include(x => x.Cases.Where(y => y.Active))
                    .ThenInclude(y => y.Reminders)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.ContactPeople)
                .Include(x => x.Jobs)
                .Include(x => x.ContactPeople)
                .Include(x => x.CashMovements)
                .Include(x => x.TimeRecords)
                .FirstOrDefault();

        public Client GetCleanClient(int clientId) =>
            _context.Clients
                .Where(x => x.Id == clientId)
                .FirstOrDefault();

        public List<Client> GetClientsWithoutCases(string profile, bool active = true) =>
            _context.Clients
                .Where(x => x.ProfileClaim == profile && active ? x.Active : !x.Active)
                .ToList();

        public async Task<Client> CreateClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClient(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public Task<int> DeleteClient(int id)
        {
            _context.Clients.Remove(_context.Clients.FirstOrDefault(x => x.Id == id));
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteProfileClients(string profile)
        {
            _context.Clients.RemoveRange(_context.Clients.Where(x => x.ProfileClaim == profile));
            return _context.SaveChangesAsync();
        }

        public int CountClients() => _context.Clients.AsNoTracking().Count(x => x.Active);

        public string GetClientName(int id) => _context.Clients.Where(x => x.Id == id).Select(y => y.Name).FirstOrDefault();

        #endregion

        #region Case

        public List<Case> GetCasesForClient(int clientId) =>
            _context.Cases
                .Where(c => c.ClientId == clientId && c.Active)
                    .Include(c => c.Reminders)
                    .Include(c => c.Notes)
                    .Include(c => c.ContactPeople)
                .ToList();

        public Case GetCase(int id) =>
            _context.Cases
                .Include(x => x.Notes)
                .Include(x => x.Reminders)
                .Where(x => x.Id == id)
                .FirstOrDefault();

        public Case GetCaseWithoutData(int id) =>
            _context.Cases
                .Where(x => x.Id == id)
                .FirstOrDefault();

        public Task<int> DeleteCase(int id)
        {
            var caseEntity = _context.Cases.FirstOrDefault(x => x.Id == id);
            _context.Cases.Remove(caseEntity);
            return _context.SaveChangesAsync();
        }

        public async Task<Case> CreateCase(int clientId, string profile, Case c)
        {
            var ClientEntity = _context.Clients
                .Include(x => x.Cases)
                .FirstOrDefault(x => x.ProfileClaim == profile && x.Id == clientId);

            ClientEntity.Cases.Add(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public async Task<Case> UpdateCase(Case c)
        {
            _context.Cases.Update(c);
            await _context.SaveChangesAsync();
            return c;
        }

        public int CountCases(int clientId) => _context.Cases.Count(x => x.ClientId == clientId);

        public string GetCaseParentName(int id) => _context.Clients.Where(x => x.Cases.Any(y => y.Id == id)).Select(x => x.Name).FirstOrDefault();

        public string GetCaseName(int id) => _context.Cases.Where(x => x.Id == id).Select(y => y.Name).FirstOrDefault();

        public string GetClientNameForCase(int id) => _context.Clients.Where(x => x.Id == _context.Cases.Where(x => x.Id == id).Select(y => y.ClientId).FirstOrDefault()).Select(y => y.Name).FirstOrDefault();

        #endregion

        #region Reminder

        public Reminder GetReminder(int id) =>
            _context.Reminders
                .Where(x => x.Id == id)
                .FirstOrDefault();

        public List<Reminder> GetReminders(string profile) =>
            _context.Clients
                .Where(x => x.ProfileClaim == profile && x.Active)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public List<Reminder> GetUpcomingReminders(string profile) =>
            _context.Clients
                .Where(x => x.ProfileClaim == profile && x.Active)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders.Where(x => x.Start >= DateTime.Now && x.Start <= DateTime.Now.AddDays(2)))
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public List<Reminder> GetRemindersForClient(int clientId) =>
            _context.Clients
                .Where(x => x.Id == clientId && x.Active)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .ToList();

        public async Task<Reminder> CreateReminder(int caseId, Reminder reminder)
        {
            var caseEntity = _context.Cases
                .Include(x => x.Reminders)
                .FirstOrDefault(x => x.Id == caseId);

            caseEntity.Reminders.Add(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        public async Task<Reminder> UpdateReminder(Reminder reminder)
        {
            _context.Reminders.Update(reminder);
            await _context.SaveChangesAsync();
            return reminder;
        }

        public Task<int> DeleteReminder(int id)
        {
            var reminder = _context.Reminders.FirstOrDefault(x => x.Id == id);
            _context.Reminders.Remove(reminder);
            return _context.SaveChangesAsync();
        }

        public Task<int> CountReminders()
        {
            throw new NotImplementedException();
        }

        public List<Reminder> GetUpcomingReminders(int clientId, DateTime startDate) =>
            _context.Clients
                .Where(x => x.Id == clientId)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .Where(x => x.Start <= startDate)
                .ToList();

        public List<Reminder> GetUpcomingReminders(string profile, DateTime startDate) =>
            _context.Clients
                .Where(x => x.ProfileClaim == profile)
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Reminders)
                .SelectMany(x => x.Cases.SelectMany(x => x.Reminders))
                .Where(x => x.Start <= startDate)
                .ToList();

        #endregion

        #region Note

        public Note GetNote(int id) => _context.Notes.AsNoTracking().FirstOrDefault(x => x.Id == id);

        public List<Note> GetNotesForCase(int caseId) =>
            _context.Notes
                .Where(x => x.CaseId == caseId)
                .ToList();

        public async Task<Note> CreateNote(int caseId, Note note)
        {
            var caseEntity = _context.Cases
                .Include(x => x.Notes)
                .FirstOrDefault(x => x.Id == caseId);

            caseEntity.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task<Note> UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public Task<int> DeleteNote(int id)
        {
            var note = _context.Notes.FirstOrDefault(x => x.Id == id);
            _context.Notes.Remove(note);
            return _context.SaveChangesAsync();
        }

        #endregion

        #region Job

        public async Task<ClientJob> CreateClientJob(int clientId, ClientJob job)
        {
            var client = _context.Clients
                .Include(x => x.Jobs)
                .FirstOrDefault(x => x.Id == clientId);

            client.Jobs.Add(job);
            await _context.SaveChangesAsync();
            return job;
        }

        public Task<int> DeleteClientJob(int id)
        {
            var job = _context.ClientJobs.FirstOrDefault(x => x.Id == id);
            _context.ClientJobs.Remove(job);
            return _context.SaveChangesAsync();
        }

        public ClientJob GetClientJob(int id) =>
            _context.ClientJobs
                .Where(x => x.Id == id)
                .FirstOrDefault();

        public List<ClientJob> GetClientJobs(int clientId) =>
            _context.ClientJobs
                .Where(x => x.ClientId == clientId)
                .ToList();

        public async Task<ClientJob> UpdateClientJob(ClientJob job)
        {
            _context.ClientJobs.Update(job);
            await _context.SaveChangesAsync();
            return job;
        }

        #endregion

        #region Cash Movements

        public CashMovement GetCashMovement(int id) => _context.CashMovements.AsNoTracking().FirstOrDefault(x => x.Id == id);

        public List<CashMovement> GetCashMovementsForClient(int clientId) =>
            _context.CashMovements
                .Where(x => x.ClientId == clientId)
                .ToList();

        public async Task<CashMovement> CreateCashMovement(int clientId, string profile, CashMovement cashMovement)
        {
            var client = _context.Clients
                .Include(x => x.CashMovements)
                .FirstOrDefault(x => x.Id == clientId && x.ProfileClaim == profile);

            client.CashMovements.Add(cashMovement);
            await _context.SaveChangesAsync();
            return cashMovement;
        }

        public async Task<CashMovement> UpdateCashMovement(CashMovement cashMovement)
        {
            _context.CashMovements.Update(cashMovement);
            await _context.SaveChangesAsync();
            return cashMovement;
        }

        public Task<int> DeleteCashMovement(int id)
        {
            var cm = _context.CashMovements.FirstOrDefault(x => x.Id == id);
            _context.CashMovements.Remove(cm);
            return _context.SaveChangesAsync();
        }

        #endregion

        #region Time Records

        public TimeRecord GetTimeRecord(int id) => _context.TimeRecords.FirstOrDefault(x => x.Id == id);

        public List<TimeRecord> GetTimeRecords(int clientId) =>
            _context.TimeRecords
                .AsNoTracking()
                .Where(x => x.ClientId == clientId)
                .ToList();

        public async Task<TimeRecord> CreateTimeRecord(int clientId, string profile, TimeRecord timeRecord)
        {
            var client = _context.Clients
                .Include(x => x.TimeRecords)
                .FirstOrDefault(x => x.Id == clientId && x.ProfileClaim == profile);

            client.TimeRecords.Add(timeRecord);
            await _context.SaveChangesAsync();
            return timeRecord;
        }

        public async Task<TimeRecord> UpdateTimeRecord(TimeRecord timeRecord)
        {
            _context.TimeRecords.Update(timeRecord);
            await _context.SaveChangesAsync();
            return timeRecord;
        }

        public Task<int> DeleteTimeRecord(int id)
        {
            var tr = _context.TimeRecords.FirstOrDefault(x => x.Id == id);
            _context.TimeRecords.Remove(tr);
            return _context.SaveChangesAsync();
        }

        #endregion

        #region Statistics

        public int CountCasesPerClient(int clientId) =>
            _context.Cases.AsNoTracking().Count(x => x.ClientId == clientId);

        public int CountJobsPerClient(int clientId) =>
            _context.ClientJobs.AsNoTracking().Count(x => x.ClientId == clientId);

        public int CountRemindersPerCase(int caseId) =>
            _context.Reminders.AsNoTracking()
                .Count(y => y.CaseId == caseId && y.End >= DateTime.Now);

        public int CountDeadlineRemindersPerCase(int caseId) =>
            _context.Reminders.AsNoTracking()
                .Count(y => y.CaseId == caseId && y.End >= DateTime.Now && y.IsDeadline);

        public int CountNotesPerCase(int caseId) =>
            _context.Notes.AsNoTracking().Count(x => x.CaseId == caseId && x.Active);

        public IEnumerable<int> GetClientCasesIds(int clientId) =>
            _context.Clients
                .AsNoTracking()
                .Include(x => x.Cases)
                .FirstOrDefault(x => x.Id == clientId && x.Active).Cases
                .Select(y => y.Id)
                .ToList();

        public IEnumerable<int> GetClientsIds(string profile) =>
            _context.Clients
                .AsNoTracking()
                .Where(x => x.ProfileClaim == profile && x.Active)
                .Select(y => y.Id)
                .ToList();

        #endregion

        #region Archive

        public int CountArchivedCases() => _context.Clients
            .Include(x => x.Cases)
            .SelectMany(x => x.Cases)
            .Where(x => !x.Active)
            .Count();

        public int CountArchivedClients() => _context.Clients.Count(x => !x.Active);

        public Task<int> ArchivizeClient(int clientId, string user)
        {
            var client = _context.Clients
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.Id == clientId)
                .FirstOrDefault();

            client.Active = false;

            foreach (var c in client.Cases)
            {
                c.Active = false;
                c.UpdatedBy = user;
                c.Updated = DateTime.Now;

                foreach (var n in c.Notes)
                {
                    n.Active = false;
                    n.UpdatedBy = user;
                    n.Updated = DateTime.Now;
                }
            }

            foreach (var j in client.Jobs)
            {
                j.Active = false;
                j.UpdatedBy = user;
                j.Updated = DateTime.Now;
            }

            _context.Clients.Update(client);
            return _context.SaveChangesAsync();
        }

        public Task<int> ArchivizeCase(int caseId, string user)
        {
            var c = _context.Cases
                .Include(x => x.Notes)
                .FirstOrDefault(x => x.Id == caseId);

            c.Active = false;
            c.UpdatedBy = user;
            c.Updated = DateTime.Now;

            foreach (var n in c.Notes)
            {
                n.Active = false;
                n.UpdatedBy = user;
                n.Updated = DateTime.Now;
            }

            _context.Cases.Update(c);
            return _context.SaveChangesAsync();
        }

        public async Task<ClientJob> ArchivizeClientJob(int jobId, string user)
        {
            var job = _context.ClientJobs.FirstOrDefault(x => x.Id == jobId);

            job.Active = false;
            job.UpdatedBy = user;
            job.Updated = DateTime.Now;

            _context.ClientJobs.Update(job);
            await _context.SaveChangesAsync();
            return job;
        }

        public Task<int> ArchivizeNote(int noteId, string user)
        {
            var note = _context.Notes.FirstOrDefault(x => x.Id == noteId);
            note.Active = false;

            _context.Notes.Update(note);
            return _context.SaveChangesAsync();
        }

        public List<Case> GetArchivedCases(int clientId) => _context.Cases.Where(x => !x.Active && x.ClientId == clientId).ToList();

        public Task<int> RecoverClient(int clientId, string user)
        {
            var client = _context.Clients
                .Include(x => x.Cases)
                    .ThenInclude(y => y.Notes)
                .Include(x => x.Jobs)
                .Where(x => x.Id == clientId)
                .FirstOrDefault();

            client.Active = true;

            foreach (var c in client.Cases)
            {
                c.Active = true;
                c.UpdatedBy = user;
                c.Updated = DateTime.Now;

                foreach (var n in c.Notes)
                {
                    n.Active = true;
                    n.UpdatedBy = user;
                    n.Updated = DateTime.Now;
                }
            }

            foreach (var j in client.Jobs)
            {
                j.Active = true;
                j.UpdatedBy = user;
                j.Updated = DateTime.Now;
            }

            _context.Clients.Update(client);
            return _context.SaveChangesAsync();
        }

        public Task<int> RecoverCase(int caseId, string user)
        {
            var c = _context.Cases
                .Include(x => x.Notes)
                .FirstOrDefault(x => x.Id == caseId);

            c.Active = true;
            c.UpdatedBy = user;
            c.Updated = DateTime.Now;

            foreach (var n in c.Notes)
            {
                n.Active = true;
                n.UpdatedBy = user;
                n.Updated = DateTime.Now;
            }

            _context.Cases.Update(c);
            return _context.SaveChangesAsync();
        }

        public async Task<ClientJob> RecoverClientJob(int jobId, string user)
        {
            var job = _context.ClientJobs.FirstOrDefault(x => x.Id == jobId);

            job.Active = true;
            job.UpdatedBy = user;
            job.Updated = DateTime.Now;

            _context.ClientJobs.Update(job);
            await _context.SaveChangesAsync();
            return job;
        }

        public Note RecoverNote(int noteId, string user)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Contact Person

        public CaseContactPerson GetCaseContactPerson(int id) => _context.CaseContactPeople.AsNoTracking().FirstOrDefault(x => x.Id == id);

        public ClientContactPerson GetClientContactPerson(int id) => _context.ClientContactPeople.AsNoTracking().FirstOrDefault(x => x.Id == id);

        public List<ClientContactPerson> GetContactPeopleForClient(int clientId) =>
            _context.ClientContactPeople
                .Where(x => x.ClientId == clientId)
                .ToList();

        public List<CaseContactPerson> GetContactPeopleForCase(int caseId) =>
            _context.CaseContactPeople
                .Where(x => x.CaseId == caseId)
                .ToList();

        public async Task<ClientContactPerson> CreateClientContactPerson(int clientId, ClientContactPerson contactPerson)
        {
            var client = _context.Clients
                .Include(x => x.ContactPeople)
                .FirstOrDefault(x => x.Id == clientId);

            client.ContactPeople.Add(contactPerson);
            await _context.SaveChangesAsync();
            return contactPerson;
        }

        public async Task<CaseContactPerson> CreateCaseContactPerson(int caseId, CaseContactPerson contactPerson)
        {
            var client = _context.Cases
                .Include(x => x.ContactPeople)
                .FirstOrDefault(x => x.Id == caseId);

            client.ContactPeople.Add(contactPerson);
            await _context.SaveChangesAsync();
            return contactPerson;
        }

        public async Task<ClientContactPerson> UpdateClientContactPerson(ClientContactPerson contactPerson)
        {
            _context.ClientContactPeople.Update(contactPerson);
            await _context.SaveChangesAsync();
            return contactPerson;
        }

        public async Task<CaseContactPerson> UpdateCaseContactPerson(CaseContactPerson contactPerson)
        {
            _context.CaseContactPeople.Update(contactPerson);
            await _context.SaveChangesAsync();
            return contactPerson;
        }

        public Task<int> DeleteClientContactPerson(int id)
        {
            var cp = _context.ClientContactPeople.FirstOrDefault(x => x.Id == id);
            _context.ClientContactPeople.Remove(cp);
            return _context.SaveChangesAsync();
        }

        public Task<int> DeleteCaseContactPerson(int id)
        {
            var cp = _context.CaseContactPeople.FirstOrDefault(x => x.Id == id);
            _context.CaseContactPeople.Remove(cp);
            return _context.SaveChangesAsync();
        }

        #endregion
    }

}
