using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ShopSphere.API.Data;
using ShopSphere.API.DTOs;
using ShopSphere.API.Exceptions;
using ShopSphere.API.Helpers;
using ShopSphere.API.Interfaces;
using ShopSphere.API.Models;
using ShopSphere.API.Repositories;
using ShopSphere.API.Repositories.Interfaces;
using System.Runtime.CompilerServices;

namespace ShopSphere.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IFileService _fileService;

        

        public ProductService(IProductRepository productRepository,IMapper mapper, ILogger<ProductService> logger, IFileService fileService, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _logger = logger;
            _fileService = fileService;
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> CreateProductAsync(CreateProductDto dto)
        {
            //var categoryexist = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
            var categoryexist = await _productRepository.ExistsAsync(dto.CategoryId);
            if (!categoryexist) 
            {
                return false;
            }

            string? imagePath = null;

            if (dto.Image != null)
            {
                imagePath = await _fileService.UploadFileAsync(dto.Image,"products");
            }

            var product = _mapper.Map<Product>(dto);
            product.ImageUrl = imagePath;


            await _productRepository.AddAsync(product);

            await _productRepository.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<ProductDto>> GetAllProductAsync(ProductQueryDto query)
        {


            //var productquery =  _context.Products.AsNoTracking().AsQueryable();
            var productquery = _productRepository.GetAll();
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                productquery = productquery.Where(p=>p.Name.Contains(query.Search));
            }

            if (query.CategoryId.HasValue) 
            {
                productquery = productquery.Where(p => p.CategoryId==query.CategoryId.Value);
            }

            if (query.MinPrice.HasValue)
            {
                productquery = productquery.Where(p => p.Price >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                productquery = productquery.Where(p => p.Price <= query.MaxPrice.Value);
            }

            productquery = (query.SortBy?.ToLower(), query.SortOrder?.ToLower()) switch
            {
                ("price", "desc") => productquery.OrderByDescending(p=>p.Price),
                ("name", "desc") => productquery.OrderByDescending(p => p.Name),
                ("price", "asc") => productquery.OrderBy(p => p.Price),
                ("name", "asc") => productquery.OrderBy(p => p.Name),
                _ => productquery.OrderByDescending(p => p.Id)
            };

            var totalRecords = await productquery.CountAsync();

            var products = await productquery.ProjectTo<ProductDto>(_mapper.ConfigurationProvider).Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToListAsync();
            _logger.LogInformation("Fetched All Products Successfully.");
            return new PagedResult<ProductDto>
            {
                Data = products,
                TotalRecords = totalRecords,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)query.PageSize)


            };

            
            
        }

        public async Task<ProductDto> GetProductByIdAsync (int Id)
        {
            //var product = await _context.Products.AsNoTracking().Where(p=> p.Id == Id).Select(p=> new ProductDto
            //{
            //    Id = p.Id,
            //    Name = p.Name,
            //    Price = p.Price, 
            //    Description = p.Description,
            //    Stock = p.Stock,
            //    CategoryName = p.Category.Name

            //}).FirstOrDefaultAsync();

            //var product = await _context.Products.AsNoTracking().Where(p => p.Id == Id).ProjectTo<ProductDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            var product = await _productRepository.GetByIdAsync(Id);
            if (product == null)
            {
                throw new NotFoundException("Product Not Found");
            }
            return _mapper.Map<ProductDto>(product);

            //return product;
        }

        public async Task<bool> UpdateProductAsync(int Id, UpdateProductDto productDto)
        {
            var product = await _productRepository.GetByIdAsync(Id);


            if (product == null) { 
            return false;
            }

            var categoryExist = await _categoryRepository.ExistsAsync(productDto.CategoryId);
            if (!categoryExist) {
                return false;
            }


            string? oldImage = product.ImageUrl;
            _mapper.Map(productDto, product);

            // Image Update
            if (productDto.Image != null)
            {
                // Delete old image
                if (!string.IsNullOrWhiteSpace(oldImage))
                {
                    _fileService.DeleteFile(oldImage);
                }

                // Upload new image
                product.ImageUrl = await _fileService.UploadFileAsync(
                                        productDto.Image,
                                        "products");
            }

            //await _context.SaveChangesAsync();
            _productRepository.Update(product);

            await _productRepository.SaveChangesAsync();
            return true;

        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            //var product = await _context.Products.FindAsync(id);
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }
             _productRepository.Delete(product);
            await _productRepository.SaveChangesAsync();
            // _context.Products.Remove(product);
            //await _context.SaveChangesAsync();
            return true;
        }

    }
}
