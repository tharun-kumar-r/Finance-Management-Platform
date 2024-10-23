using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebFinanceApi.Functions
{
    public class Functions
    {

        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static int GenerateAccountNumber(int nextId)
        {
            Random random = new Random();
            return int.Parse($"{nextId}{random.Next(100, 999)}"); 
        }


        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
               
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

             
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x3"));
                }
                return builder.ToString();
            }
        }
    }


}
