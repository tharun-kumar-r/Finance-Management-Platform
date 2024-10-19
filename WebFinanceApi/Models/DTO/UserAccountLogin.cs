using System.ComponentModel.DataAnnotations;

namespace WebFinanceApi.Models.DTO
{
    public class UserAccountLogin
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
