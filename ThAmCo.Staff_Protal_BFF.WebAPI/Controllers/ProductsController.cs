﻿using DTOs;
using Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.Interfaces.Products;

namespace ThAmCo.Staff_Protal_BFF.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductsService _productsService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductsService ProductsData, ILogger<ProductsController> Logger)
        {
            _productsService = ProductsData;
            _logger = Logger;
        }

        [HttpGet]
        [Route("GetAllProducts")]
        public ActionResult<List<ProductsDTO>> GetAllProducts()
        {
            try
            {
                var result = _productsService.GetAllAvailableProducts();

                if (result == null)
                {
                    return StatusCode(500, "Failed to retrieve all products from the database.");
                }

                return Ok(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                  new EventId((int)LogEventIdEnum.UnknownError),
                  $"Unexpected exception was caught in ProductsController at GetAllProducts() .\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server..");
            }
        }

        [HttpPost]
        [Route("GetAllCategoryProducts")]
        public ActionResult<List<ProductsDTO>> GetAllCategoryProducts([FromBody] string categoryName)
        {
            try
            {
                var result = _productsService.GetAllCategoryProducts(categoryName);

                if (result == null)
                {
                    return StatusCode(500, "Failed to retrieve specified category products from the database.");
                }

                return Ok(JsonConvert.SerializeObject(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                  new EventId((int)LogEventIdEnum.UnknownError),
                  $"Unexpected exception was caught in ProductsController at GetAllCategoryProducts().\nException:\n{ex.Message}\nInner exception:\n{ex.InnerException}\nStack trace:\n{ex.StackTrace}");

                return StatusCode(500, "Server error. An unknown error occurred on the server..");
            }
        }
    }
}
