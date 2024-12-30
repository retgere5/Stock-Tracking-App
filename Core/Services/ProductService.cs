using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesktopApp.Core.Entities;
using DesktopApp.Infrastructure.Data.Context;
using DesktopApp.Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;

namespace DesktopApp.Core.Services;

/// <summary>
/// Service class for product operations
/// </summary>
public class ProductService : IBaseService<Product>
{
    private readonly DatabaseContext _context;

    public ProductService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.StockMovements)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<Product> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.StockMovements)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
            throw new ArgumentException($"Product with ID {id} not found.");

        return product;
    }

    public async Task<Product> AddAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required.");

        if (product.Price < 0)
            throw new ArgumentException("Product price cannot be negative.");

        if (product.CurrentStock < 0)
            throw new ArgumentException("Product stock cannot be negative.");

        product.CreatedAt = DateTime.Now;
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        Logger.LogInfo($"Added new product: {product.Name}");
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            throw new ArgumentException("Product name is required.");

        if (product.Price < 0)
            throw new ArgumentException("Product price cannot be negative.");

        if (product.CurrentStock < 0)
            throw new ArgumentException("Product stock cannot be negative.");

        var existingProduct = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        if (existingProduct == null)
            throw new ArgumentException($"Product with ID {product.Id} not found.");

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        Logger.LogInfo($"Updated product: {product.Name}");
        return existingProduct;
    }

    public async Task DeleteAsync(int id)
    {
        var product = await GetByIdAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Search products by name
    /// </summary>
    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _context.Products
            .Include(p => p.StockMovements)
            .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }
} 