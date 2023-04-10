using Catalog.API.Interfaces.Manager;
using Catalog.API.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : BaseController
    {
        private readonly IProductManager productManager;

        public CatalogController(IProductManager productManager)
        {
            this.productManager = productManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ResponseCache(Duration = 10)]
        public IActionResult GetProducts()
        {
            try
            {
                var products = productManager.GetAll();
                return CustomResult("Data Loaded Successfully", products, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.Created)]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                bool isSaved = productManager.Add(product);

                if (isSaved)
                {
                    return CustomResult("Product has been saved successfully!", product ,HttpStatusCode.Created);
                }
                return CustomResult("Product saved failed!", product, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Id))
                {
                    return CustomResult("Data not found!", HttpStatusCode.NotFound);
                }
                bool isModified = productManager.Update(product.Id, product);

                if (isModified)
                {
                    return CustomResult("Product has been updated successfully!", product, HttpStatusCode.OK);
                }
                return CustomResult("Product update failed!", product, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }


        [HttpDelete]
        public IActionResult DeleteProduct(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return CustomResult("Data not found!", HttpStatusCode.NotFound);
                }
                bool isDeleted = productManager.Delete(id);

                if (isDeleted)
                {
                    return CustomResult("Product has been deleted successfully!", HttpStatusCode.OK);
                }
                return CustomResult("Product delete failed!", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
