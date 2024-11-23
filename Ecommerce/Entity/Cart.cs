using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Entity
{
    public class Cart
    {
        public int CartId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Cart() { }

        public Cart(int cartId, int customerId, int productId, int quantity)
        {
            CartId = cartId;
            CustomerId = customerId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}

