using System.ComponentModel.DataAnnotations;

namespace ProductApi.DTO
{
    public class ProductDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 100000)]
        public int Quantity { get; set; }
    }
}