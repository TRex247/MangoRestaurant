using AutoMapper;
using Mango.Services.ProductApi.DbContexts;
using Mango.Services.ProductApi.Models;
using Mango.Services.ProductApi.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductApi.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            Product product = await _db.Products.Where(x=> x.ProductId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            IEnumerable<Product> productList = await _db.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(productList);
        }
    }
}
