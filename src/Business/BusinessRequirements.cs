using DAL.Repositories;
using DTO.V1.WholesalerRequest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business
{
    public static class BusinessRequirements
    {
        public static async Task EnsureAllRequirementsAsync(int wholesalerId, Order order, IRepository<Models.V1.Wholesaler> repository)
        {
            EnsureOrderDetailsIsNotEmpty(order.OrderDetails);

            await EnsureWholesalerExistsAsync(wholesalerId, repository);

            EnsureOrderDetailsAreUnique(order.OrderDetails);

            await EnsureBeersAreSoldByWholesalerAsync(wholesalerId, order.OrderDetails, repository);

            await EnsureQuantitiesAreNotGreaterThanStockAsync(wholesalerId, order.OrderDetails, repository);
        }

        public static void EnsureOrderDetailsIsNotEmpty(IList<Stock> orderDetails)
        {
            if (orderDetails == null || orderDetails.Count() == 0) 
                throw new BusinessException(ErrorCodes.ORDER_CANNOT_BE_EMPTY, nameof(orderDetails));
        }

        public static async Task<Models.V1.Wholesaler> EnsureWholesalerExistsAsync(int wholesalerId, IRepository<Models.V1.Wholesaler> repository)
        {
            var wholesaler = await repository.ReadAsync(wholesalerId);
            if (wholesaler == null) throw new BusinessException(ErrorCodes.WHOLESALER_MUST_EXIST, nameof(wholesalerId));
            return wholesaler;
        }

        public static void EnsureOrderDetailsAreUnique(IList<Stock> orderDetails)
        {
            var duplicates = orderDetails.GroupBy(x => x.BeerId);
            if (duplicates.Any(g => g.Count() > 1)) 
                throw new BusinessException(ErrorCodes.NO_DUPLICATE_IN_ORDER, nameof(duplicates));
        }

        public static async Task EnsureBeersAreSoldByWholesalerAsync(int wholesalerId, IList<Stock> orderDetails, IRepository<Models.V1.Wholesaler> repository)
        {
            var wholesaler = await EnsureWholesalerExistsAsync(wholesalerId, repository);
            var notSold = orderDetails.Select(od => od.BeerId).Except(wholesaler.Stocks.Select(s => s.Beer.Id));
            if (notSold.Any()) {
                var beer = notSold.FirstOrDefault();
                throw new BusinessException(ErrorCodes.BEER_NOT_SOLD_BY_WHOLESALER, nameof(beer));
            }
        }

        public static async Task EnsureBeersAreNotSoldByWholesalerAsync(int wholesalerId, IList<Stock> orderDetails, IRepository<Models.V1.Wholesaler> repository)
        {
            var wholesaler = await EnsureWholesalerExistsAsync(wholesalerId, repository);
            var sold = orderDetails.Select(od => od.BeerId).Intersect(wholesaler.Stocks.Select(s => s.Beer.Id));
            if (sold.Any())
            {
                var beer = sold.FirstOrDefault();
                throw new BusinessException(ErrorCodes.BEER_ALREADY_SOLD_BY_WHOLESALER, nameof(beer));
            }
        }

        public static async Task EnsureQuantitiesAreNotGreaterThanStockAsync(int wholesalerId, IList<Stock> orderDetails, IRepository<Models.V1.Wholesaler> repository)
        {
            var wholesaler = await EnsureWholesalerExistsAsync(wholesalerId, repository);
            if (!orderDetails.All(details => 
                    wholesaler.Stocks.FirstOrDefault(s => s.Beer.Id == details.BeerId)?.Quantity >= details.Quantity
                )
            ) throw new BusinessException(ErrorCodes.NOT_GREATER_NUMBER_THAN_IN_STOCK, "stock");
        }

        public static async Task<Models.V1.Brewery> EnsureBreweryExistsAsync(int breweryId, IRepository<Models.V1.Brewery> repository)
        {
            var brewery = await repository.ReadAsync(breweryId);
            if (brewery == null) throw new BusinessException(ErrorCodes.BREWERY_MUST_EXIST, nameof(breweryId));
            return brewery;
        }
    }
}
