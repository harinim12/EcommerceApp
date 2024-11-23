using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Ecommerce.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException() : base("Product not found!") { }

        public ProductNotFoundException(string message) : base(message) { }

        public ProductNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
