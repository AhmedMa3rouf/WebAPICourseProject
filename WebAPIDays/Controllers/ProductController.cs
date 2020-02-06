using BusinessEntities;
using BusinessServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDays.Filters;

namespace WebAPIDays.Controllers
{
    [ApiAuthenticationFilter]
    [RoutePrefix("Api/Product")]
    public class ProductController : ApiController
    {
        readonly IProductServices productService;
       
        public ProductController(IProductServices productServices)
        {
            this.productService = productServices;
            // This line if before dependancy injection using Unity.MVC5
            //productService = new ProductServices();
        }
        // GET: api/Product
        [Route("AllProducts")]
        public HttpResponseMessage Get()
        {
            var products = productService.GetAllProducts();
            var productEntites = products as List<ProductEntity> ?? products.ToList();
            if (productEntites.Any())
                return Request.CreateResponse(HttpStatusCode.OK, productEntites);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No Products");
        }

        // GET: api/Product/5
        public HttpResponseMessage Get(int id)
        {
            var product = productService.GetProductById(id);
            if (product != null)
                return Request.CreateResponse(HttpStatusCode.OK, product);
            return Request.CreateErrorResponse(HttpStatusCode.NotFound, $"Product with ID: {id} not exists");
        }

        // POST: api/Product
        public int Post([FromBody]ProductEntity productEntity)
        {
            return productService.CreateProduct(productEntity);
        }

        // PUT: api/Product/5
        public bool Put(int id, [FromBody]ProductEntity productEntity)
        {
            if (id > 0)
            {
                return productService.UpdateProduct(id, productEntity);
            }
            return false;
        }

        // DELETE: api/Product/5
        public bool Delete(int id)
        {
            if (id > 0)
                return productService.DeleteProduct(id);
            return false;
        }
    }
}
