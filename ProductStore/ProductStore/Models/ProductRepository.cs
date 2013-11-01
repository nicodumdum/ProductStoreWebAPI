using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductStore.Models
{
    public class ProductRepository : IProductRepository
    {
        private MySQLDBDriver mySQLDBDriver = new MySQLDBDriver(
            "48ea4334-f908-439d-9155-a26801228547.mysql.sequelizer.com",
            "lomrjnnynsbwqkca",
            "GQLGjgixD8aMome4GwRTxxXCbbBPccZw4PzqfKcuyekvE2RuMguqNtLi2hEhLBuA",
            "db48ea4334f908439d9155a26801228547");

        public ProductRepository()
        {
            //List<Product> products = new List<Product>();
            //products.Add(new Product { Id = 1, Name = "Tomato soup", Category = "Groceries", Price = 1.39M });
            //products.Add(new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M });
            //products.Add(new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M });
            //Add(new Product { Name = "Tomato soup", Category = "Groceries", Price = 1.39M });
            //Add(new Product { Name = "Yo-yo", Category = "Toys", Price = 3.75M });
            //Add(new Product { Name = "Hammer", Category = "Hardware", Price = 16.99M });
            //foreach (var product in products)
            //{
            //    if (Get(product.Id) == null)
            //    {
            //        Add(product);
            //    }
            //}
        }

        public IEnumerable<Product> GetAll()
        {
            return mySQLDBDriver.getAllProducts();
        }

        public Product Get(int id)
        {
            return mySQLDBDriver.getProduct(id);
        }

        public Product Add(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            mySQLDBDriver.addProduct(item);
            return item;
        }

        public void Remove(int id)
        {
            mySQLDBDriver.deleteProduct(id);
        }

        public bool Update(Product item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (mySQLDBDriver.getProduct(item.Id) == null)
            {
                return false;
            }
            mySQLDBDriver.updateProduct(item);
            return true;
        }
    }
}