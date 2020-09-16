using DAL.Extensions;
using DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.V1;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class BreweryRepository : RepositoryBase<Brewery>
    {
        public BreweryRepository(BreweryDbContext context, ILogger<BreweryRepository> logger) : base(context, logger)
        {
        }

        protected new DbSet<Brewery> GetDbSet()
        {
            return Context.Breweries;
        }

        public new Task<Brewery> ReadAsync(int entityId)
        {
            return GetDbSet()
                .Include(br => br.Beers)
                    .ThenInclude(b => b.Stocks)
                        .ThenInclude(s => s.Wholesaler)
                .FirstOrDefaultAsync(Beer => Beer.Id == entityId);
        }

        public new Task<Brewery[]> ReadAllWithFilterAsync(string filter = null, int pageIndex = Constants.PageIndex, int pageSize = Constants.PageSize)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(filter);
            if (hasFilter) filter = filter.ToLower();

            return GetDbSet()
                .Include(br => br.Beers)
                    .ThenInclude(b => b.Stocks)
                        .ThenInclude(s => s.Wholesaler)
                .Where(br => !hasFilter
                    || br.Name.ToLower().Contains(filter)
                    || br.Beers.Any(b => b.Name.ToLower().Contains(filter))
                    || br.Beers.SelectMany(b => b.Stocks).Any(s => s.Wholesaler.Name.ToLower().Contains(filter)))
                .OrderBy(br => br.Id)
                .TakePage(pageIndex, pageSize)
                .ToArrayAsync();
        }
    }
}
