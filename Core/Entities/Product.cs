using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DesktopApp.Core.Entities
{
    public class Product : BaseModel
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0, 999999.99)]
        public decimal Price { get; set; }

        [Required]
        public int CurrentStock { get; set; }

        public virtual ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
    }
} 