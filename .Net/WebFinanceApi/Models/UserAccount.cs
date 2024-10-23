using System.ComponentModel.DataAnnotations;

namespace WebFinanceApi.Models
{
    public class UserAccount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AccountNo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
