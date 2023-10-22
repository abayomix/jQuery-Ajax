using Microsoft.EntityFrameworkCore;

namespace jQuery_Ajax.Models
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options)
        {

        }
        public DbSet<Transaction> Transactions { get; set; }

    }
}
