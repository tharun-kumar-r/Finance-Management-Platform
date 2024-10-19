using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Globalization;
using WebFinanceApi.Models;
using WebFinanceApi.Models.DTO;

namespace WebFinanceApi.Controllers
{
    public class TransactionController : Controller
    {

        private readonly DatabaseContext _dbcontext;

        public TransactionController(DatabaseContext context)
        {
            _dbcontext = context;
        }



        [HttpGet]
        [Route("/transaction/balance")]
        public IActionResult Balance(int AccountNo, string TokenNo)
        {

            var exittoken = _dbcontext.SessionTokens.FirstOrDefault(u => u.TokenNo == TokenNo && u.AccountNo == AccountNo);
            if (exittoken == null)
            {
                return BadRequest(new { message = "Invalid Session" });
            }         

            var balance = _dbcontext.transactions
             .Where(t => t.AccountNo == AccountNo && t.Status == 1)
             .Sum(t => t.TrnsType == 1 ? t.Amount : -t.Amount); 

            return Ok(balance);
          
        }

        [HttpPost]
        [Route("/transaction/send")]
        public IActionResult send(int FromAccountNo, string TokenNo, int toAccount, int amount)
        {
            var exittoken = _dbcontext.SessionTokens.FirstOrDefault(u => u.TokenNo == TokenNo && u.AccountNo == FromAccountNo);
            if (exittoken == null || amount < 1 )
            {
                return BadRequest(new { message = "Invalid Session!." });
            }

            if (FromAccountNo == toAccount)
            {
                return BadRequest(new { message = "You cannot send Amount to same Account!." });
            }

            var existingUser = _dbcontext.userAccounts.FirstOrDefault(u => u.AccountNo == toAccount);
            if (existingUser == null)
            {
                return BadRequest(new { message = "To Account not found!." });
            }


            var balance = _dbcontext.transactions
            .Where(t => t.AccountNo == FromAccountNo && t.Status == 1)
            .Sum(t => t.TrnsType == 1 ? t.Amount : -t.Amount);
            if (balance == null || balance == 0 || balance < amount)
            {
                return BadRequest(new { message = "Insufficient Balance" });
            }
            
                var transactioncredit = new Transaction
                {
                    TrnsType = 1,
                    TrnsId = Guid.NewGuid().ToString(),
                    AccountNo = toAccount,
                    To = FromAccountNo,
                    Status = 1,
                    Amount = amount
                };


                _dbcontext.transactions.Add(transactioncredit);
                _dbcontext.SaveChanges();

            var transactiondbit = new Transaction
            {
                TrnsType = 0,
                TrnsId = Guid.NewGuid().ToString(),
                AccountNo = FromAccountNo ,
                To = toAccount,
                Status = 1,
                Amount = amount
            };


            _dbcontext.transactions.Add(transactiondbit);
            _dbcontext.SaveChanges();

            return Ok(new { message = "Transaction successful!" });
           

        }


        [HttpPost]
        [Route("/transaction/credit")]
        public IActionResult credit(int AccountNo, string TokenNo, string sourceName, int amount)
        {

            var exittoken = _dbcontext.SessionTokens.FirstOrDefault(u => u.TokenNo == TokenNo && u.AccountNo == AccountNo);
            if (exittoken == null || amount < 1 || sourceName == "")
            {
                return BadRequest(new { message = "Invalid Session!." });
            }
            int randomNumber = new Random().Next(2143214214);

            var transactioncredit = new Transaction
            {
                TrnsType = 1,
                TrnsId = Guid.NewGuid().ToString("N"),
                AccountNo = AccountNo,
                To = randomNumber,
                SourceName = sourceName,
                Status = 1,
                Amount = amount
            };


            _dbcontext.transactions.Add(transactioncredit);
            _dbcontext.SaveChanges();


            return Ok(new { message = "Transaction successful!" });
        }

        [HttpGet]
        [Route("/transaction/history")]
        public IActionResult History(int AccountNo, string TokenNo, string type, DateTime? startdate = null, DateTime? enddate = null)
        {
           
            var exittoken = _dbcontext.SessionTokens.FirstOrDefault(u => u.TokenNo == TokenNo && u.AccountNo == AccountNo);
            if (exittoken == null || type == null)
            {
                return BadRequest(new { message = "Invalid Session!" });
            }

           
            if (startdate == null || enddate == null)
            {
                startdate = DateTime.Now.AddDays(-10);
                enddate = DateTime.Now; 
            }

            IQueryable<TransactionDto> transactionsQuery = _dbcontext.transactions
                .Where(x => x.AccountNo == AccountNo && x.Date >= startdate && x.Date <= enddate)
                .Select(x => new TransactionDto
                {
                    TrnsId = x.TrnsId,
                    TrnsType = x.TrnsType == 1 ? "Credit" : "Debit",
                    Amount = x.Amount,
                    Source = x.SourceName == null ? $"{x.To}: {_dbcontext.userAccounts.Where(y => y.AccountNo == x.To).Select(y => y.Name).FirstOrDefault()}" : $"{x.To}: {x.SourceName}",
                    Status = x.Status == 1 ? "Success" : "Failed",
                    Date = x.Date
                });

           
            switch (type.ToLower())
            {
                case "all":
                    break; 
                case "credit":
                    transactionsQuery = transactionsQuery.Where(x => x.TrnsType == "Credit");
                    break;
                case "debit":
                    transactionsQuery = transactionsQuery.Where(x => x.TrnsType == "Debit");
                    break;
                default:
                    return BadRequest(new { message = "Invalid Transaction Type!" });
            }

            
            var transactions = transactionsQuery.ToList();
            return Ok(transactions);
        }

        [HttpGet]
        [Route("/transaction/graph")]
        public IActionResult graph(int AccountNo, string TokenNo)
        {
            var exittoken = _dbcontext.SessionTokens.FirstOrDefault(u => u.TokenNo == TokenNo && u.AccountNo == AccountNo);
            if (exittoken == null)
            {
                return BadRequest(new { message = "Invalid Session!" });
            }

           
                var transactions = _dbcontext.transactions
                    .Where(x => x.AccountNo == AccountNo)
                    .Select(x => new
                    {
                        Type = x.TrnsType == 1 ? "Credit" : "Debit",
                        x.Amount,
                        x.Date
                    })
                    .ToList();

                return Ok(transactions);
          
         
          
        }


    }
}
