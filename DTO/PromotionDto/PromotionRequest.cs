using System.ComponentModel.DataAnnotations;

namespace DTO.PromotionDto
{
    public class PromotionRequest
    {
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Discount is required")]
        [RegularExpression(@"^(?!0+(\.0*)?%?$)(\d{1,2}(\.\d+)?%$|100(\.0+)?%)$", ErrorMessage = "Discount must be a number from 1 to 100, followed by a '%' symbol.")]
        public string? Discount { get; set; }
        [Required(ErrorMessage = "Start date is required")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "End date is required")]
        public DateTime? EndDate { get; set; }
        public DateTime CreateAt { get; set; }
        public bool? IsActive { get; set; }
    }
}
