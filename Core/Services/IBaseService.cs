using System.Threading.Tasks;
using System.Collections.Generic;
using DesktopApp.Core.Entities;

namespace DesktopApp.Core.Services;

/// <summary>
/// Base interface for all services
/// </summary>
/// <typeparam name="T">Model type</typeparam>
public interface IBaseService<T> where T : BaseModel
{
    /// <summary>
    /// Get all records
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Get record by ID
    /// </summary>
    Task<T> GetByIdAsync(int id);

    /// <summary>
    /// Add new record
    /// </summary>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Update existing record
    /// </summary>
    Task<T> UpdateAsync(T entity);

    /// <summary>
    /// Delete record
    /// </summary>
    Task DeleteAsync(int id);
} 