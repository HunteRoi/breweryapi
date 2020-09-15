using Models.V1;
using System.Collections.Generic;
using System.Linq;

namespace DTO.V1.WholesalerRequest
{
    public class Quote
    {
        private Quote()
        {
            Summary = new List<string>();
        }

        public Quote(Order order, IEnumerable<Beer> beers) : this()
        {           
            Cost = order.OrderDetails.Sum(s =>
            {
                var beer = beers.FirstOrDefault(b => b.Id == s.BeerId);
                return s.Quantity * beer.Price;
            });

            ApplyPromotions(order.OrderDetails.Sum(details => details.Quantity));
        }

        public decimal Cost { get; set; }

        public IEnumerable<string> Summary { get; set; }

        public Quote AddMessage(string msg)
        {
            ((List<string>)Summary).Add(msg);
            return this;
        }

        private void ApplyPromotions(int beersCount)
        {
            if (beersCount > 20)
            {
                Cost = CalculatePromotion(Cost, 0.2m);
                AddMessage("A discount of 20% is applied above 20 drinks");
            }
            else if (beersCount > 10)
            {
                Cost = CalculatePromotion(Cost, 0.1m);
                AddMessage("A discount of 10% is applied above 10 drinks");
            }
        }

        private static decimal CalculatePromotion(decimal price, decimal percent)
        {
            return price - (price * percent);
        }
    }
}
