using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductStore.Models;

namespace ProductStore.Controllers
{
    public class ProductsController : ApiController
    {
        static readonly IProductRepository repository = new ProductRepository();

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>
        /// Returns an <c>IEnumerable</c> collection of <c>Product</c> objects.
        /// </returns>
        public IEnumerable<Product> GetAllProducts()
        {
            return repository.GetAll();
        }

        /// <summary>
        /// Gets the product.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>
        /// Returns a <c>Product</c> object based on the input <c>id</c> integer.
        /// </returns>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        public Product GetProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return item;
        }

        /// <summary>
        /// Gets the products by category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>
        /// Returns an <c>IEnumerable</c> collection of <c>Product</c> objects based on the input <c>category</c> string.
        /// </returns>
        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return repository.GetAll().Where(
                p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Posts the product.
        /// </summary>
        /// <param name="item">The product item.</param>
        /// <returns>
        /// Returns a <c>HttpResponseMessage</c> regarding the result of the method operation.
        /// </returns>
        public HttpResponseMessage PostProduct(Product item)
        {
            item = repository.Add(item);
            var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }

        /// <summary>
        /// Puts the product.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <param name="product">The product.</param>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        public void PutProduct(int id, Product product)
        {
            product.Id = id;
            if (!repository.Update(product))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Deletes the product.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        public void DeleteProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            repository.Remove(id);
        }
    }
}
