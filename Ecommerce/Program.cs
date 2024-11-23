using System;
using System.Collections.Generic;
using ECommerce.Repository;
using ECommerce.Entity;
using ECommerce.Exceptions;

namespace ECommerce
{
    class Program
    {
        private IOrderProcessorRepository repository;

        public Program()
        {
            repository = new OrderProcessorRepositoryImpl();
        }

        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
        }

        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== E-Commerce Application ===");
                Console.WriteLine("1. Register Customer");
                Console.WriteLine("2. Create Product");
                Console.WriteLine("3. Add Product to Cart");
                Console.WriteLine("4. View Cart");
                Console.WriteLine("5. Place Order");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": RegisterCustomer(); break;
                    case "2": CreateProduct(); break;
                    case "3": AddToCart(); break;
                    case "4": ViewCart(); break;
                    case "5": PlaceOrder(); break;
                    case "6": running = false; break;
                    default: Console.WriteLine("Invalid choice!"); break;
                }
            }

            Console.WriteLine("Thank you for using the application.");
        }

        private void RegisterCustomer()
        {
            Console.Write("Enter Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Email: ");
            string email = Console.ReadLine();
            Console.Write("Enter Password: ");
            string password = Console.ReadLine();

            var customer = new Customer { Name = name, Email = email, Password = password };
            bool result = repository.RegisterCustomer(customer);
            Console.WriteLine(result ? "Customer registered successfully!" : "Registration failed.");
            Console.ReadLine();
        }

        private void CreateProduct()
        {
            Console.Write("Enter Product Name: ");
            string name = Console.ReadLine();
            Console.Write("Enter Price: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter Description: ");
            string description = Console.ReadLine();
            Console.Write("Enter Stock Quantity: ");
            int stock = int.Parse(Console.ReadLine());

            var product = new Product { Name = name, Price = price, Description = description, StockQuantity = stock };
            bool result = repository.CreateProduct(product);
            Console.WriteLine(result ? "Product created successfully!" : "Product creation failed.");
            Console.ReadLine();
        }

        private void AddToCart()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());
            Console.Write("Enter Product ID: ");
            int productId = int.Parse(Console.ReadLine());
            Console.Write("Enter Quantity: ");
            int quantity = int.Parse(Console.ReadLine());

            bool result = repository.AddToCart(customerId, productId, quantity);
            Console.WriteLine(result ? "Product added to cart!" : "Failed to add product to cart.");
            Console.ReadLine();
        }

        private void ViewCart()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());

            var items = repository.GetCartItems(customerId);
            Console.WriteLine("Cart Items:");
            foreach (var item in items)
                Console.WriteLine($"Product: {item.Name}, Price: {item.Price:C}, Quantity: {item.StockQuantity}");

            Console.ReadLine();
        }

        private void PlaceOrder()
        {
            Console.Write("Enter Customer ID: ");
            int customerId = int.Parse(Console.ReadLine());
            Console.Write("Enter Shipping Address: ");
            string address = Console.ReadLine();

            var items = repository.GetCartItems(customerId);
            var orderItems = new List<(int productId, int quantity)>();
            foreach (var item in items)
                orderItems.Add((item.ProductId, item.StockQuantity));

            bool result = repository.PlaceOrder(customerId, orderItems, address);
            Console.WriteLine(result ? "Order placed successfully!" : "Failed to place order.");
            Console.ReadLine();
        }
    }
}
