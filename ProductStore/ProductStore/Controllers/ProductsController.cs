using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductStore.Models;
using ProductStore.Filters;
using System.Diagnostics;

namespace ProductStore.Controllers
{
    public class ProductsController : ApiController
    {
        /* IMPORTANT NOTE ABOUT HOW THE BASIC ROUTING WORKS WITH METHOD NAMES:
         * To find the action with the basic routing, Web API looks at the HTTP method,
         * and then looks for an action whose name begins with that HTTP method name.
         * For example, with a GET request, Web API looks for an action that starts with "Get...",
         * such as "GetContact" or "GetAllContacts".
         * This convention applies only to GET, POST, PUT, and DELETE methods.
         * You can enable other HTTP methods by using attributes on your controller.
         * 
         * To prevent a method from getting invoked as an action, use the NonAction attribute.
         * This signals to the framework that the method is not an action,
         * even if it would otherwise match the routing rules.
         * // Not an action method.
         * [NonAction]  
         * public string GetPrivateData() { ... }
         */

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
        public Product GetProduct(int id)
        {
            Product item = repository.Get(id);
            if (item == null)
            {
                //var message = string.Format("Product with id = {0} not found", id);
                //HttpError err = new HttpError(message);
                //return Request.CreateErrorResponse(HttpStatusCode.NotFound, err);

                var message = string.Format("Product with id = {0} not found", id);
                throw new HttpResponseException(
                    Request.CreateErrorResponse(HttpStatusCode.NotFound, message));
            }
            else
            {
                //return Request.CreateResponse(HttpStatusCode.OK, item);
                return item;
            }
        }

        //Uses exception filters.
        //An exception filter is executed when a controller method throws any unhandled exception that 
        //is not an HttpResponseException exception. The HttpResponseException type is a special case, 
        //because it is designed specifically for returning an HTTP response.

        /// <summary>
        /// Just a sample method that is not yet implemented.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns>
        /// Returns a <c>NotImplementedException</c> object.
        /// </returns>
        /// <exception cref="System.NotImplementedException">This method is not implemented</exception>
        //[NotImplExceptionFilter]
        //public Product GetProduct(int id)
        //{
        //    throw new NotImplementedException("This method is not implemented");
        //}
        
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
        [ValidateModel]
        public HttpResponseMessage PostProduct(Product item)
        {
            if (ModelState.IsValid)
            {
                // Do something with the product.
                item = repository.Add(item);
                var response = Request.CreateResponse<Product>(HttpStatusCode.Created, item);

                string uri = Url.Link("DefaultApi", new { id = item.Id });
                response.Headers.Location = new Uri(uri);
                return response;

                //return new HttpResponseMessage(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
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
