using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Ecommerce.Exceptions;
using Ecommerce.Utility;
using ECommerce.Entity;
using ECommerce.Exceptions;
using ECommerce.Repository;



namespace ECommerce.Repository
{
    public class OrderProcessorRepositoryImpl : IOrderProcessorRepository
    {
        private readonly string connectionString;
        private readonly SqlCommand cmd;

        public OrderProcessorRepositoryImpl()
        {
            connectionString = DbConnUtil.GetConnectionString(); // Fetch the connection string
            cmd = new SqlCommand();
        }

        public bool RegisterCustomer(Customer customer)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "INSERT INTO customers (cname, email, cpassword) VALUES (@Name, @Email, @Password)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", customer.Name);
                cmd.Parameters.AddWithValue("@Email", customer.Email);
                cmd.Parameters.AddWithValue("@Password", customer.Password);
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool CreateProduct(Product product)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "INSERT INTO products (pname, price, pdescription, stockQuantity) VALUES (@Name, @Price, @Description, @StockQuantity)";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Description", product.Description);
                cmd.Parameters.AddWithValue("@StockQuantity", product.StockQuantity);
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "DELETE FROM customers WHERE customer_id = @CustomerId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                var result = cmd.ExecuteNonQuery();
                if (result == 0)
                    throw new CustomerNotFoundException($"Customer with ID {customerId} does not exist.");
                return true;
            }
        }

        public bool DeleteProduct(int productId)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "DELETE FROM products WHERE product_id = @ProductId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                var result = cmd.ExecuteNonQuery();
                if (result == 0)
                    throw new ProductNotFoundException($"Product with ID {productId} does not exist.");
                return true;
            }
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "SELECT * FROM products";
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductId = (int)reader["product_id"],
                        Name = (string)reader["pname"],
                        Price = (decimal)reader["price"],
                        Description = (string)reader["pdescription"],
                        StockQuantity = (int)reader["stockQuantity"]
                    };
                    products.Add(product);
                }
            }
            return products;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "SELECT * FROM customers";
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        CustomerId = (int)reader["customer_id"],
                        Name = (string)reader["cname"],
                        Email = (string)reader["email"],
                        Password = (string)reader["cpassword"]
                    };
                    customers.Add(customer);
                }
            }
            return customers;
        }

        public bool AddToCart(int customerId, int productId, int quantity)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = "INSERT INTO cart (customer_id, product_id, quantity) VALUES (@CustomerId, @ProductId, @Quantity)";
                cmd.Parameters.Clear();

                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<Product> GetCartItems(int customerId)
        {
            List<Product> products = new List<Product>();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = @"
                    SELECT p.product_id, p.pname, p.price, p.pdescription, p.stockQuantity
                    FROM cart c
                    JOIN products p ON c.product_id = p.product_id
                    WHERE c.customer_id = @CustomerId";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        ProductId = (int)reader["product_id"],
                        Name = (string)reader["pname"],
                        Price = (decimal)reader["price"],
                        Description = (string)reader["pdescription"],
                        StockQuantity = (int)reader["stockQuantity"]
                    };
                    products.Add(product);
                }
            }
            return products;
        }

        public bool PlaceOrder(int customerId, List<(int productId, int quantity)> items, string shippingAddress)
        {
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var transaction = sqlConnection.BeginTransaction())
                {
                    try
                    {
                        cmd.Connection = sqlConnection;
                        cmd.Transaction = transaction;

                        cmd.CommandText = "INSERT INTO orders (customer_id, order_date, total_price, shipping_address) OUTPUT INSERTED.order_id VALUES (@CustomerId, GETDATE(), @TotalPrice, @ShippingAddress)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@TotalPrice", CalculateTotalPrice(items));
                        cmd.Parameters.AddWithValue("@ShippingAddress", shippingAddress);

                        int orderId = (int)cmd.ExecuteScalar();

                        foreach (var (productId, quantity) in items)
                        {
                            cmd.CommandText = "INSERT INTO order_items (order_id, product_id, quantity) VALUES (@OrderId, @ProductId, @Quantity)";
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.Parameters.AddWithValue("@ProductId", productId);
                            cmd.Parameters.AddWithValue("@Quantity", quantity);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        public List<(Product product, int quantity)> GetOrdersByCustomer(int customerId)
        {
            List<(Product product, int quantity)> orders = new List<(Product product, int quantity)>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                cmd.CommandText = @"
                    SELECT 
                        p.product_id, 
                        p.pname, 
                        p.price, 
                        p.pdescription, 
                        oi.quantity
                    FROM 
                        orders o
                    JOIN 
                        order_items oi ON o.order_id = oi.order_id
                    JOIN 
                        products p ON oi.product_id = p.product_id
                    WHERE 
                        o.customer_id = @CustomerId";

                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@CustomerId", customerId);
                cmd.Connection = sqlConnection;

                sqlConnection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            ProductId = (int)reader["product_id"],
                            Name = (string)reader["pname"],
                            Price = (decimal)reader["price"],
                            Description = (string)reader["pdescription"]
                        };

                        int quantity = (int)reader["quantity"];
                        orders.Add((product, quantity));
                    }
                }
            }

            return orders;
        }
    }
}
       
    }
}