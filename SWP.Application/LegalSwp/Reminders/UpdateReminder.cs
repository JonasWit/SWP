using SWP.Domain.Infrastructure.LegalApp;
using SWP.Domain.Models.LegalApp;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class UpdateReminder
    {
        private readonly ILegalManager legalSwpManager;
        public UpdateReminder(ILegalManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<Reminder> Update(Request request)
        {
            var reminderEntity = legalSwpManager.GetReminder(request.Id);

            reminderEntity.IsDeadline = request.IsDeadline;
            reminderEntity.Name = request.Name;
            reminderEntity.Priority = request.Priority;
            reminderEntity.Updated = DateTime.Now;
            reminderEntity.UpdatedBy = request.UpdatedBy;
            reminderEntity.Message = request.Message;
            reminderEntity.Start = request.Start;
            reminderEntity.End = request.End;

            return legalSwpManager.UpdateReminder(reminderEntity);
        }

        public class Request
        {
            public int Id { get; set; }
            [MaxLength(50)]
            [Required]
            public string Name { get; set; }
            [MaxLength(500)]
            public string Message { get; set; }
            public int Priority { get; set; }
            public bool IsDeadline { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
            public DateTime Updated { get; set; }
            [MaxLength(50)]
            [Required]
            public string UpdatedBy { get; set; }
        }
    }
}
