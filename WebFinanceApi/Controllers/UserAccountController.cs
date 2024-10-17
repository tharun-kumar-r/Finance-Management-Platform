using WebFinanceApi.Models.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFinanceApi.Models;

namespace WebFinanceApi.Controllers
{
    public class UserAccountController : Controller
    {

        private readonly DatabaseContext _dbcontext;

        public UserAccountController(DatabaseContext context)
        {
            _dbcontext = context;
        }


        [Route("/useraccount/create")]
        [HttpPost]
        public IActionResult Create([FromBody] UserAccountDto UserAccountDto)
        {


            if (!Functions.Functions.IsValidEmail(UserAccountDto.Email))
            {
                return BadRequest(new { message = "Invalid email format." });
            }

            if (string.IsNullOrWhiteSpace(UserAccountDto.Password) || UserAccountDto.Password.Length < 6)
            {
                return BadRequest(new { message = "Password must be at least 6 characters long." });
            }
            var existingUser = _dbcontext.userAccounts.FirstOrDefault(u => u.Email == UserAccountDto.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email already exists." });
            }

            var lastUser = _dbcontext.userAccounts.OrderByDescending(u => u.Id).FirstOrDefault();
            int nextId = (lastUser != null) ? lastUser.Id + 1 : 1;
            int accountNumber = Functions.Functions.GenerateAccountNumber(nextId);
            var newUser = new UserAccount
            {
                Name = UserAccountDto.Name,
                Email = UserAccountDto.Email,
                Phone = UserAccountDto.Phone,
                Address = UserAccountDto.Address,
                Password = Functions.Functions.HashPassword(UserAccountDto.Password),
                AccountNo = accountNumber
            };
            _dbcontext.userAccounts.Add(newUser);
            _dbcontext.SaveChanges();

            return Ok(new { message = "Account created successfully", accountNo = accountNumber });

        }


        [Route("/useraccount/login")]
        [HttpPost]
        public IActionResult Login([FromBody] UserAccountLogin UserAccountLogin)
        {

            string password = Functions.Functions.HashPassword(UserAccountLogin.Password);

            var existingUser = _dbcontext.userAccounts
         .FirstOrDefault(u => u.Email == UserAccountLogin.Email && u.Password == password);

            if (existingUser != null)
            {


                int hash = password.GetHashCode();


                SessionToken session = new SessionToken
                {
                    TokenNo = hash.ToString(),   
                    AccountNo = existingUser.AccountNo   
                };

                _dbcontext.SessionTokens.Add(session);
                _dbcontext.SaveChanges();
                
                return Ok(new { message = "Login Success!.", authcode = hash });



            }
            else
            {

                return Conflict(new { message = "Invalid Login Details!." });


            }





        }


        [Route("/useraccount/update")]
        [HttpPost]
        public IActionResult Update([FromBody] UserAccountUpdateDto userAccountDto)
        {
            var exittoken = _dbcontext.SessionTokens.FirstOrDefault(u => u.TokenNo == userAccountDto.TokenNo && u.AccountNo == userAccountDto.AccountNo);
            if (exittoken == null)
            {
                return BadRequest(new { message = "Invalid Session" });
            }

            var existingUser = _dbcontext.userAccounts.FirstOrDefault(u => u.AccountNo == userAccountDto.AccountNo);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found." });
            }


            if (!Functions.Functions.IsValidEmail(userAccountDto.Email))
            {
                return BadRequest(new { message = "Invalid email format." });
            }

            var emailConflictUser = _dbcontext.userAccounts.FirstOrDefault(u => u.Email == userAccountDto.Email && u.AccountNo != userAccountDto.AccountNo);
            if (emailConflictUser != null)
            {
                return Conflict(new { message = "Email already exists for another account." });
            }


            if (!string.IsNullOrWhiteSpace(userAccountDto.Password) && userAccountDto.Password.Length < 6)
            {
                return BadRequest(new { message = "Password must be at least 6 characters long." });
            }


            existingUser.Name = userAccountDto.Name;
            existingUser.Email = userAccountDto.Email;
            existingUser.Phone = userAccountDto.Phone;
            existingUser.Address = userAccountDto.Address;


            if (!string.IsNullOrWhiteSpace(userAccountDto.Password))
            {
                existingUser.Password = Functions.Functions.HashPassword(userAccountDto.Password);
            }


            _dbcontext.SaveChanges();

            return Ok(new { message = "Account updated successfully." });
        }


        [Route("/useraccount/get")]
        [HttpGet]
        public IActionResult Getuser( string TokenNo, int AccountNo)
        {
            var exittoken = _dbcontext.SessionTokens.FirstOrDefault(u => u.TokenNo == TokenNo && u.AccountNo == AccountNo);
            if (exittoken == null)
            {
              return BadRequest(new { message = "Invalid Session" });
            }

            var existingUser = _dbcontext.userAccounts.FirstOrDefault(u => u.AccountNo == AccountNo);
            if (existingUser == null)
            {
                return NotFound(new { message = "User not found." });
            }
            else
            {
                UserAccountDto dto = new UserAccountDto
                {
                    
                    Name = existingUser.Name,
                    Email = existingUser.Email,
                    Phone = existingUser.Phone,
                    Address = existingUser.Address,
                   
                };

                return Ok(dto);
            }

        }
    }
}