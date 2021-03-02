using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using AddressREST.Models;
using AddressREST.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AddressREST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        // private IRepairService RepairService { get; set; }
        
        private IBaseRepository<AddressProduct> Products { get; set; }

        public MainController(IBaseRepository<AddressProduct> products )
        {
            Products = products;
        }

        [HttpGet]
        public JsonResult Get()
        {
            return new JsonResult(Products.GetAll());
        }

        [HttpPost]
        public JsonResult Post()
        {
            return new JsonResult("Work was successfully done");
        }

        [HttpPut]
        public JsonResult Put(AddressProduct addr)
        {
            bool success = true;
            var product = Products.Get(addr.Id);
            try
            {
                if (product != null)
                {
                    product = Products.Update(addr);
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult($"Update successful {addr.Id}") : 
                new JsonResult("Update was not successful");
        }

        [HttpDelete]
        public JsonResult Delete(Guid id)
        {
            bool success = true;
            var product = Products.Get(id);

            try
            {
                if (product != null)
                {
                    Products.Delete(product.Id);
                }
                else
                {
                    success = false;
                }
            }
            catch (Exception)
            {
                success = false;
            }

            return success ? new JsonResult("Delete successful") : 
                new JsonResult("Delete was not successful");
        }
    }
}