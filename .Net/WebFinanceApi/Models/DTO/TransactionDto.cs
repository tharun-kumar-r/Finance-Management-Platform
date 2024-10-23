using System.ComponentModel.DataAnnotations;

namespace WebFinanceApi.Models.DTO
{
    public class TransactionDto
    {
        [Required]
        public string TrnsId { get; set; }
        [Required]
        public string TrnsType { get; set; }
        [Required]
        public long Amount { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }

}
