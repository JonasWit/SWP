﻿using SWP.Domain.Models.BaseClasses;
using SWP.Domain.Models.LegalApp.AccessControl;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.LegalApp
{
    public class Case : BaseModel
    {
        public bool Active { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [MaxLength(100)]
        public string Signature { get; set; }
        [MaxLength(100)]
        public string CaseType { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        public int Priority { get; set; }
        public List<Reminder> Reminders { get; set; }
        public List<Note> Notes { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public List<CaseContactPerson> ContactPeople { get; set; }
        public List<AccessToCase> AccessesToCase { get; set; }
    }
}
