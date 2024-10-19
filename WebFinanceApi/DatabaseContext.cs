using WebFinanceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace WebFinanceApi
{
  

    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserAccount> userAccounts { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }

        public DbSet<Transaction> transactions { get; set; }

        


    }
}
