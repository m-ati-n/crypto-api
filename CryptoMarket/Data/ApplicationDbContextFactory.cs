using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CryptoMarket.Data
{
    public class ApplicationDbContextFactory
        : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            optionsBuilder.UseNpgsql(
            "Host=tokaido.proxy.rlwy.net;Port=30457;Database=railway;Username=postgres;Password=xwwpTrQEllgUurgvSEbAcHVtHLlhWfgA"
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}