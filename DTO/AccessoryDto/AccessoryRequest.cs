using DTO.Media;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTO.AccessoryDto
{
    public class AccessoryRequest
	{
		[Required(ErrorMessage = "Name is required")]
        [MaxLength(100)]
        [RegularExpression(@"^[^\d]*$", ErrorMessage = "Name cannot contain numbers")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
        public Guid? PromotionId { get; set; }
        public MediaRequest? Media { get; set; }
    }
}
