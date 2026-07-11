using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampaignManagement.Models.Entity
{
    [Table("trnExpenses")]
    public class trnExpense
    {
        [Key]
        public int trnExpenseId { get; set; }
        
        [Required]
        public int campaignId { get; set; }
        
        [Required]
        public string expenseName { get; set; } = string.Empty;
        
        public string? category { get; set; }
        public string? paidTo { get; set; }
        
        public decimal amount { get; set; } = 0.00m;
        
        public string status { get; set; } = "Pending";
        
        public DateTime? expenseDate { get; set; }
        public string? proofPath { get; set; }
        
        public bool isActive { get; set; } = true;
    }
}
