using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CMSApp.Models;

namespace CMSApp.Data
{
    public class CMSDBContext : IdentityDbContext<Users>
    {
        public CMSDBContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
