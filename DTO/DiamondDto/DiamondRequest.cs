using System.ComponentModel.DataAnnotations;

namespace DTO.DiamondDto
{
    public class DiamondRequest
    {
        [Required(ErrorMessage = "Origin is required")]
        public string Origin { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string Color { get; set; }

        [Required(ErrorMessage = "Cut is required")]
        public string Cut { get; set; }

        [Required(ErrorMessage = "Clarity is required")]
        public string Clarity { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})$", ErrorMessage = "Weight must be a number with one or two decimal places")]
        public string Weight { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal Price { get; set; }
    }
}
