using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using WebApi.DTOs;

namespace Infrastructure.Services;


public class ProductService : IProductService
{
    private readonly DataContext _context;
    public ProductService(DataContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        return await _context.Products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price
        }).ToListAsync();
    }
    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var p = await _context.Products.FindAsync(id);
        return p == null ? null : new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price
        };
    }
    public async Task<ProductDto> CreateAsync(ProductCreateUpdateDto dto)
    {
        var product = new Products
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
    }
    public async Task<bool> UpdateAsync(int id, ProductCreateUpdateDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;
        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}
