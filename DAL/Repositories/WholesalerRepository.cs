using DAL.Extensions;
using DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.V1;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class WholesalerRepository : RepositoryBase<Wholesaler>
    {
        public WholesalerRepository(BreweryDbContext context, ILogger<WholesalerRepository> logger) : base(context, logger)
        {
        }

        protected new DbSet<Wholesaler> GetDbSet()
        {
            return Context.Wholesalers;
        }

        public new Task<Wholesaler> ReadAsync(int entityId)
        {
            return GetDbSet()
                .Include(ws => ws.Stocks)
                    .ThenInclude(s => s.Beer)
                .FirstOrDefaultAsync(Beer => Beer.Id == entityId);
        }

        public new Task<Wholesaler[]> ReadAllWithFilterAsync(string filter = null, int pageIndex = Constants.PageIndex, int pageSize = Constants.PageSize)
        {
            bool hasFilter = !string.IsNullOrWhiteSpace(filter);
            if (hasFilter) filter = filter.ToLower();

            return GetDbSet()
                .Include(ws => ws.Stocks)
                    .ThenInclude(s => s.Beer)
                .Where(br => !hasFilter || br.Name.ToLower().Contains(filter))
                .OrderBy(br => br.Id)
                .TakePage(pageIndex, pageSize)
                .ToArrayAsync();
        }
    }
}
