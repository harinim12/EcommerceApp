using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ECommerce.Exceptions
{
    public class CustomerNotFoundException : Exception
    {
        public CustomerNotFoundException() : base("Customer not found!") { }

        public CustomerNotFoundException(string message) : base(message) { }

        public CustomerNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
