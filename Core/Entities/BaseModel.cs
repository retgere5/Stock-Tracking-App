namespace DesktopApp.Core.Entities;

/// <summary>
/// Base class for all model classes
/// </summary>
public abstract class BaseModel
{
    /// <summary>
    /// Unique identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Creation date
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    /// <summary>
    /// Last update date
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 