using System;
using System.Data.SqlClient;
using Ecommerce.Utility;

namespace ECommerce.Repository
{
    public class TestDatabaseConnection
    {
        public void TestConnection()
        {
            string connectionString = DbConnUtil.GetConnectionString();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                try
                {
                    sqlConnection.Open();
                    Console.WriteLine("Connection successful!");
                }
                catch (SqlException ex)
                {
                    Console.WriteLine($"Error connecting to the database: {ex.Message}");
                }
            }
        }
    }
}
