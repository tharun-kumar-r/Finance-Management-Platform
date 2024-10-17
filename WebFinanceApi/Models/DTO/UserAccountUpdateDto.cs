namespace WebFinanceApi.Models.DTO
{
    public class UserAccountUpdateDto
    {
        public string TokenNo { get; set; }
        public int AccountNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        public string Password { get; set; }

    }
}
