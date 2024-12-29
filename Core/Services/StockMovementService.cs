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
/// Service class for stock movement operations
/// </summary>
public class StockMovementService : IBaseService<StockMovement>
{
    private readonly DatabaseContext _context;

    public StockMovementService(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockMovement>> GetAllAsync()
    {
        return await _context.StockMovements
            .Include(sm => sm.Product)
            .OrderByDescending(sm => sm.CreatedAt)
            .ToListAsync();
    }

    public async Task<StockMovement> GetByIdAsync(int id)
    {
        var movement = await _context.StockMovements
            .Include(sm => sm.Product)
            .FirstOrDefaultAsync(sm => sm.Id == id);

        return movement ?? throw new KeyNotFoundException($"Stock movement with ID {id} not found");
    }

    public async Task<StockMovement> AddAsync(StockMovement movement)
    {
        var product = await _context.Products.FindAsync(movement.ProductId)
            ?? throw new KeyNotFoundException($"Product with ID {movement.ProductId} not found");

        // Update product stock quantity
        product.CurrentStock += movement.Type == MovementType.Out ? -movement.Quantity : movement.Quantity;
        product.UpdatedAt = DateTime.Now;

        // Add movement
        movement.CreatedAt = DateTime.Now;
        _context.StockMovements.Add(movement);
        await _context.SaveChangesAsync();

        Logger.LogInfo($"Added stock movement: {movement.Type} - {movement.Quantity} units of {product.Name}");
        return movement;
    }

    public async Task<StockMovement> UpdateAsync(StockMovement movement)
    {
        var existingMovement = await GetByIdAsync(movement.Id);
        var product = await _context.Products.FindAsync(movement.ProductId)
            ?? throw new KeyNotFoundException($"Product with ID {movement.ProductId} not found");

        // Revert old stock quantity
        product.CurrentStock -= existingMovement.Type == MovementType.Out 
            ? -existingMovement.Quantity 
            : existingMovement.Quantity;

        // Apply new stock quantity
        product.CurrentStock += movement.Type == MovementType.Out 
            ? -movement.Quantity 
            : movement.Quantity;

        product.UpdatedAt = DateTime.Now;
        movement.UpdatedAt = DateTime.Now;

        _context.Entry(existingMovement).CurrentValues.SetValues(movement);
        await _context.SaveChangesAsync();

        Logger.LogInfo($"Updated stock movement: {movement.Type} - {movement.Quantity} units of {product.Name}");
        return movement;
    }

    public async Task DeleteAsync(int id)
    {
        var movement = await GetByIdAsync(id);
        var product = await _context.Products.FindAsync(movement.ProductId)
            ?? throw new KeyNotFoundException($"Product with ID {movement.ProductId} not found");

        // Revert stock quantity
        product.CurrentStock -= movement.Type == MovementType.Out 
            ? -movement.Quantity 
            : movement.Quantity;

        product.UpdatedAt = DateTime.Now;

        _context.StockMovements.Remove(movement);
        await _context.SaveChangesAsync();

        Logger.LogInfo($"Deleted stock movement: {movement.Type} - {movement.Quantity} units of {product.Name}");
    }

    /// <summary>
    /// Get stock movements for a specific product
    /// </summary>
    public async Task<IEnumerable<StockMovement>> GetByProductAsync(int productId)
    {
        return await _context.StockMovements
            .Include(sm => sm.Product)
            .Where(sm => sm.ProductId == productId)
            .OrderByDescending(sm => sm.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Get stock movements by date range
    /// </summary>
    public async Task<IEnumerable<StockMovement>> GetByDateRangeAsync(DateTime start, DateTime end)
    {
        return await _context.StockMovements
            .Include(sm => sm.Product)
            .Where(sm => sm.CreatedAt >= start && sm.CreatedAt <= end)
            .OrderByDescending(sm => sm.CreatedAt)
            .ToListAsync();
    }

    /// <summary>
    /// Get stock movements by type
    /// </summary>
    public async Task<IEnumerable<StockMovement>> GetByTypeAsync(MovementType type)
    {
        return await _context.StockMovements
            .Include(sm => sm.Product)
            .Where(sm => sm.Type == type)
            .OrderByDescending(sm => sm.CreatedAt)
            .ToListAsync();
    }
} 