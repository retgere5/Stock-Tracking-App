using System;
using System.ComponentModel.DataAnnotations;

namespace DesktopApp.Core.Entities
{
    /// <summary>
    /// Stock movement model class
    /// </summary>
    public class StockMovement : BaseModel
    {
        /// <summary>
        /// Movement type (in, out, adjustment)
        /// </summary>
        [Required]
        public MovementType Type { get; set; }

        /// <summary>
        /// Quantity of movement
        /// </summary>
        [Required]
        [Range(-999999, 999999)]
        public int Quantity { get; set; }

        /// <summary>
        /// Reference number (invoice, order, etc.)
        /// </summary>
        [MaxLength(100)]
        public string? Reference { get; set; }

        /// <summary>
        /// Notes about the movement
        /// </summary>
        [MaxLength(500)]
        public string? Notes { get; set; }

        /// <summary>
        /// Related product ID
        /// </summary>
        [Required]
        public int ProductId { get; set; }

        /// <summary>
        /// User who made the movement
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; } = string.Empty;

        /// <summary>
        /// Related product
        /// </summary>
        public virtual Product Product { get; set; } = null!;
    }
} 