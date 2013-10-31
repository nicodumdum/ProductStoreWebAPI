using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ProductStore.Models
{
    public class MySQLDBDriver
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="MySQLDBDriver"/> class.
        /// </summary>
        /// <param name="dbServer">The database server.</param>
        /// <param name="dbUsername">The database username.</param>
        /// <param name="dbPassword">The database password.</param>
        /// <param name="dbName">Name of the database.</param>
        public MySQLDBDriver(string dbServer, string dbUsername, string dbPassword, string dbName)
        {
            this.connectionString =
                "server=" + dbServer + ";" +
                "port=3306;" +
                "uid=" + dbUsername + ";" +
                "pwd=" + dbPassword + ";" +
                "database=" + dbName + ";";
        }

        /// <summary>
        /// Checks if the database connection is ok.
        /// </summary>
        /// <returns>
        /// Returns <c>true</c> if the database connection is ok; otherwise, <c>false</c>.
        /// </returns>
        public bool dbConnectionOk()
        {
            bool result = false;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    result = true;
                }
                catch (MySqlException ex)
                {
                    switch (ex.Number)
                    {
                        case 4060: // Invalid Database 
                            break;
                        case 18456: // Login Failed 
                            break;
                        default:
                            break;
                    }
                }
            }
            return result;
        }
        
        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>
        /// Returns an <c>IEnumerable</c> collection of <c>Product</c> objects if at least one product exists; otherwise, returns <c>null</c>.
        /// </returns>
        public IEnumerable<Product> getAllProducts()
        {
            string sqlStatement = "SELECT * FROM products ORDER BY id ASC;";
            DataTable dataTable = new DataTable();
            List<Product> products = new List<Product>();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, sqlStatement))
            {
                dataTable.Load(reader);
            }
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    products.Add(new Product {Id = int.Parse(row[0].ToString()), Name = row[1].ToString(), Category = row[2].ToString(), Price = decimal.Parse(row[3].ToString()) });
                }
                return products;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>
        /// Returns a <c>Product</c> object based on the input <c>id</c> integer if it exists; otherwise, returns <c>null</c>.
        /// </returns>
        public Product getProduct(int id)
        {
            string sqlStatement = "SELECT * FROM products WHERE id = @id;";
            MySqlParameter[] parameters = new MySqlParameter[1];
            parameters[0] = new MySqlParameter("id", id);
            DataTable dataTable = new DataTable();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, sqlStatement, parameters))
            {
                dataTable.Load(reader);
            }
            if (dataTable.Rows.Count > 0)
            {
                DataRow row = dataTable.Rows[0];
                return new Product { Id = int.Parse(row[0].ToString()), Name = row[1].ToString(), Category = row[2].ToString(), Price = decimal.Parse(row[3].ToString()) };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets all products by category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>
        /// Returns an <c>IEnumerable</c> collection of <c>Product</c> objects based on the input <c>category</c> string if at least one product exists; otherwise, returns <c>null</c>.
        /// </returns>
        public IEnumerable<Product> getAllProductsByCategory(string category)
        {
            string sqlStatement = "SELECT * FROM products WHERE category = @category ORDER BY id ASC;";
            MySqlParameter[] parameters = new MySqlParameter[1];
            parameters[0] = new MySqlParameter("category", category);
            DataTable dataTable = new DataTable();
            List<Product> products = new List<Product>();
            using (MySqlDataReader reader = MySqlHelper.ExecuteReader(connectionString, sqlStatement, parameters))
            {
                dataTable.Load(reader);
            }
            if (dataTable.Rows.Count > 0)
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    products.Add(new Product { Id = int.Parse(row[0].ToString()), Name = row[1].ToString(), Category = row[2].ToString(), Price = decimal.Parse(row[3].ToString()) });
                }
                return products;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Adds the product.
        /// </summary>
        /// <param name="product">The product.</param>
        /// <returns>
        /// Returns the integer count value of affected rows by the non-query operation.
        /// </returns>
        public int addProduct(Product product)
        {
            string sqlStatement = "INSERT INTO products (id, name, category, price) VALUES (NULL, @name, @category, @price);";
            MySqlParameter[] parameters = new MySqlParameter[3];
            parameters[0] = new MySqlParameter("name", product.Name);
            parameters[1] = new MySqlParameter("category", product.Category);
            parameters[2] = new MySqlParameter("price", product.Price);
            return MySqlHelper.ExecuteNonQuery(connectionString, sqlStatement, parameters);
        }

        /// <summary>
        /// Updates the product.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="product">The product.</param>
        /// <returns>
        /// Returns the integer count value of affected rows by the non-query operation.
        /// </returns>
        public int updateProduct(Product product)
        {
            string sqlStatement = "UPDATE products SET name = @name, category = @category, price = @price WHERE id =  @id;";
            MySqlParameter[] parameters = new MySqlParameter[4];
            parameters[0] = new MySqlParameter("id", product.Id);
            parameters[1] = new MySqlParameter("name", product.Name);
            parameters[2] = new MySqlParameter("category", product.Category);
            parameters[3] = new MySqlParameter("price", product.Price);
            return MySqlHelper.ExecuteNonQuery(connectionString, sqlStatement, parameters);
        }

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>
        /// Returns the integer count value of affected rows by the non-query operation.
        /// </returns>
        public int deleteProduct(int id)
        {
            string sqlStatement = "DELETE FROM products WHERE id = @id;";
            MySqlParameter[] parameters = new MySqlParameter[1];
            parameters[0] = new MySqlParameter("id", id);
            return MySqlHelper.ExecuteNonQuery(connectionString, sqlStatement, parameters);
        }
    }
}