using DTO.Media;
using DTO.PromotionDto;
using System.ComponentModel.DataAnnotations;

namespace DTO.AccessoryDto
{
    public class AccessoryResponse
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Name cannot contain numbers")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal? Price { get; set; }
        public Guid? PromotionId { get; set; }
        public Guid? MediaId { get; set; }
        public PromotionResponse? Promotion { get; set; }
        public MediaResponse? Media { get; set; }
    }
}