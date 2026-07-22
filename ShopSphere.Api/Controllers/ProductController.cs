using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopSphere.API.DTOs;
using ShopSphere.API.DTOs.Auth;
using ShopSphere.API.Enums;
using ShopSphere.API.Interfaces;
using ShopSphere.API.Responses;
using ShopSphere.API.Services;
using System.Security.Claims;

namespace ShopSphere.API.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    
    public class ProductController : Controller
    {
        private readonly IProductService _productservice;

        private readonly ICurrentUserService _currentUserService;

        public ProductController(IProductService productservice, ICurrentUserService currentUserService)
        {
            _productservice = productservice;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        //[Authorize(Roles="Admin")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productDto)
        {
           

            var productcreated = await _productservice.CreateProductAsync(productDto);

            if (!productcreated)
            {
                return BadRequest("Category Does Not Exist");
            }

            return NoContent();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts([FromQuery] ProductQueryDto query)
        {
            var products = await _productservice.GetAllProductAsync(query);
            return Ok(products);


        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productservice.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            //return Ok(product);
            return Ok(ApiResponseFactory.Success(product, "Product fetched successfully."));

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, UpdateProductDto updateProductDto)
        {
            var updated = await _productservice.UpdateProductAsync(id, updateProductDto);
            if (!updated)
            {
                return BadRequest("Invalid Product Id Or Categtory Id");
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var deleted = await _productservice.DeleteProductAsync(id);

            if (!deleted)
            {
                return BadRequest("Product Not Found");
            }
            return NoContent();
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<CurentUserDto> Me()
        {
            return Ok(_currentUserService.GetCurrentUser());
        }


    }
}
