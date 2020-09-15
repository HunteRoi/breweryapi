using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.V1.WholesalerRequest
{
    public class Order
    {
        [Required]
        public List<Stock> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new List<Stock>();
        }
    }
}
