using Microsoft.EntityFrameworkCore;
using Models.V1;

namespace DAL
{
    public class BreweryDbContext : DbContext
    {

        public DbSet<Beer> Beers { get; set; }
        public DbSet<Brewery> Breweries { get; set; }
        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Wholesaler> Wholesalers { get; set; }

        public BreweryDbContext(DbContextOptions<BreweryDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
