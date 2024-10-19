﻿using System.ComponentModel.DataAnnotations;

namespace WebFinanceApi.Models.DTO
{
    public class UserAccountDto
    {
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
