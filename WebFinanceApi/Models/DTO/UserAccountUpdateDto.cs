using System.ComponentModel.DataAnnotations;

namespace WebFinanceApi.Models.DTO
{
    public class UserAccountUpdateDto
    {
        [Required]
        public string TokenNo { get; set; }
        [Required]
        public int AccountNo { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
