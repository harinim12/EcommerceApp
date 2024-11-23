using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string ShippingAddress { get; set; }

        public Order() { }

        public Order(int orderId, int customerId, DateTime orderDate, decimal totalPrice, string shippingAddress)
        {
            OrderId = orderId;
            CustomerId = customerId;
            OrderDate = orderDate;
            TotalPrice = totalPrice;
            ShippingAddress = shippingAddress;
        }
    }
}
