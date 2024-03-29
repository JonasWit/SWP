﻿using SWP.Domain.Models.BaseClasses;
using System;
using System.ComponentModel.DataAnnotations;

namespace SWP.Domain.Models.LegalApp
{
    public class CashMovement : BaseModel
    {
        public double Amount { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        public bool Expense { get; set; }
        public DateTime EventDate { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
