using InventoryApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace InventoryApplication.Controllers
{
    public class ProductController : ApiController
    {
        private static ProductDBEntities dbContext = new ProductDBEntities();

        // GET api/values
        public async Task<HttpResponseMessage> Get()
        {
            try
            {
                var lstproducts = await dbContext.Products.ToListAsync();
                if (lstproducts != null && lstproducts.Count() > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, lstproducts);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products are not available");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // GET api/values/5
        public async Task<HttpResponseMessage> Get(int id)
        {
            try
            {
                var product = await dbContext.Products.Where(x => x.ProductID == id).FirstOrDefaultAsync();
                if (product != null)
                {

                    return Request.CreateResponse(HttpStatusCode.OK, product);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with Id = " + id.ToString() + " not found");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // POST api/values
        public async Task<HttpResponseMessage> Post([FromBody] ProductViewModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product entity = new Product();
                    entity.ProductName = product.ProductName;
                    entity.ProductDescription = product.ProductDescription;
                    entity.ProductPrice = product.ProductPrice;
                    entity.ProductExpiryDate = product.ProductExpiryDate;

                    dbContext.Products.Add(entity);
                    await dbContext.SaveChangesAsync();

                    return Request.CreateResponse(HttpStatusCode.Created);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }



        // PUT api/values/1
        public async Task<HttpResponseMessage> Put(int id, [FromBody] ProductViewModel product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var entity = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with Id = " + id.ToString() + "not found");
                    }
                    else
                    {
                        entity.ProductName = product.ProductName;
                        entity.ProductDescription = product.ProductDescription;
                        entity.ProductPrice = product.ProductPrice;
                        entity.ProductExpiryDate = product.ProductExpiryDate;
                        await dbContext.SaveChangesAsync();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        // DELETE api/values/1
        public async Task<HttpResponseMessage> Delete(int id)
        {
            try
            {
                var entity = await dbContext.Products.FirstOrDefaultAsync(x => x.ProductID == id);
                if (entity == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Product with id =" + id.ToString() + " not found to delete");
                }
                else
                {
                    dbContext.Products.Remove(entity);
                    await dbContext.SaveChangesAsync();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
