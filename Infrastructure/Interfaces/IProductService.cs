using WebApi.DTOs;

namespace Infrastructure.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(ProductCreateUpdateDto dto);
    Task<bool> UpdateAsync(int id, ProductCreateUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}