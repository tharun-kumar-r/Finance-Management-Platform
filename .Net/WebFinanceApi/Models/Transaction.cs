using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace WebFinanceApi.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TrnsType { get; set; }
        [Required]
        public string TrnsId { get; set; }
        [Required]
        public int AccountNo { get; set; }
        [Required]
        public int To { get; set; }
        [Required]
        public int Status { get; set; }
        [Required]
        public long Amount { get; set; }

        public string? SourceName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public Transaction()
        {
        
            Date = DateTime.Now;
        }
    }
}
