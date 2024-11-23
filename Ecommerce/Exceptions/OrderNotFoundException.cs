using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException() : base("Order not found!") { }

        public OrderNotFoundException(string message) : base(message) { }

        public OrderNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
