using System.ComponentModel.DataAnnotations;

namespace WebFinanceApi.Models
{
    public class SessionToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountNo { get; set; }

        [Required]
        public string TokenNo { get; set; }
    }
}
