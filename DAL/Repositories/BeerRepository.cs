using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using DTOs = DTO.V1.BeerRequest;
using Models.V1;
using System.Linq;
using System.Threading.Tasks;
using DTO;
using DAL.Extensions;
using System.Collections.Generic;

namespace DAL.Repositories
{
    public class BeerRepository : RepositoryBase<Beer>
    {
        public BeerRepository(BreweryDbContext context, ILogger<BeerRepository> logger) : base(context, logger)
        {
        }

        protected new DbSet<Beer> GetDbSet()
        {
            return Context.Beers;
        }

        public Beer Add(DTOs.Beer beer)
        {
            var brewery = Context.Breweries.FirstOrDefault(br => br.Id == beer.BreweryId);
            return base.Add(new Beer(beer.Name, beer.AlcoholLevel, beer.Price, brewery));
        }

        public async Task<Beer> AddAsync(DTOs.Beer beer)
        {
            var brewery = await Context.Breweries.FirstOrDefaultAsync(br => br.Id == beer.BreweryId);
            var beerModel = await base.AddAsync(new Beer(beer.Name, beer.AlcoholLevel, beer.Price, brewery));
            return beerModel;
        }

        public override Task<Beer[]> ReadAllWithFilterAsync(string filter = null, int pageIndex = Constants.PageIndex, int pageSize = Constants.PageSize)
        {
            var hasFilter = !string.IsNullOrWhiteSpace(filter);
            if (hasFilter) filter = filter.ToLower();

            return Task.FromResult(GetDbSet()
                .Include(b => b.Brewery)
                .Where(b => !hasFilter
                    || b.Name.ToLower().Contains(filter)
                    || b.Brewery.Name.ToLower().Contains(filter)
                )
                .TakePage(pageIndex, pageSize)
                .ToArray());
        }

        public Task<IEnumerable<Beer>> ReadAllFromListAsync(IEnumerable<int> providedBeerIds)
        {
            return Task.FromResult(Context.Beers
                .Where(b => providedBeerIds.Contains(b.Id))
                .AsEnumerable()
            );
        }
    }
}
