using System.Collections.Generic;
using ECommerce.Entity;

namespace ECommerce.Repository
{
    public interface IOrderProcessorRepository
    {
        bool RegisterCustomer(Customer customer);
        bool CreateProduct(Product product);
        bool DeleteCustomer(int customerId);
        bool DeleteProduct(int productId);
        List<Product> GetProducts();
        List<Customer> GetCustomers();
        bool AddToCart(int customerId, int productId, int quantity);
        List<Product> GetCartItems(int customerId);
        bool PlaceOrder(int customerId, List<(int productId, int quantity)> items, string shippingAddress);
        List<(Product product, int quantity)> GetOrdersByCustomer(int customerId);
    }
}
