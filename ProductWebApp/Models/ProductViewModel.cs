using System.ComponentModel.DataAnnotations;

namespace ProductWebApp.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 100000)]
        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}