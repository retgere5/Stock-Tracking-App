using System;

namespace DesktopApp.Core.Entities;

/// <summary>
/// Event arguments for product-related events
/// </summary>
public class ProductEventArgs : EventArgs
{
    public Product Product { get; }
    public string Message { get; }
    public bool Success { get; }

    public ProductEventArgs(Product product, string message = "", bool success = true)
    {
        Product = product;
        Message = message;
        Success = success;
    }
} 