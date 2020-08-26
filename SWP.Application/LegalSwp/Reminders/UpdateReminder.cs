using SWP.Domain.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace SWP.Application.LegalSwp.Reminders
{
    [TransientService]
    public class UpdateReminder
    {
        private readonly ILegalSwpManager legalSwpManager;
        public UpdateReminder(ILegalSwpManager legalSwpManager) => this.legalSwpManager = legalSwpManager;

        public Task<int> Update(Request request)
        {
            var reminderEntity = legalSwpManager.GetReminder(request.Id, x => x);

            reminderEntity.IsDeadline = request.IsDeadline;
            reminderEntity.Name = request.Name;
            reminderEntity.Active = true;
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
