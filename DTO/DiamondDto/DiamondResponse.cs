using DTO.DiamondAccessoryDto;
using DTO.Media;
using DTO.PaperworkDto;
using DTO.PromotionDto;
using System.ComponentModel.DataAnnotations;

namespace DTO.DiamondDto
{
    public class DiamondResponse
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Origin is required")]
        public string? Origin { get; set; }

        [Required(ErrorMessage = "Color is required")]
        public string? Color { get; set; }

        [Required(ErrorMessage = "Cut is required")]
        public string? Cut { get; set; }

        [Required(ErrorMessage = "Clarity is required")]
        [RegularExpression(@"^(?!0+(\.0*)?$)(\d{1,2}(\.\d+)?|100(\.0+)?)$", ErrorMessage = "Clarity must be a decimal number greater than 0 and less than 100")]
        public string? Clarity { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [RegularExpression(@"^(?!0+(\.0*)?$)\d+(\.\d{1,})?$", ErrorMessage = "Weight must be a decimal number greater than 0")]
        public string? Weight { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal? Price { get; set; }

        public Guid? MediaId { get; set; }
        public bool? IsSold { get; set; }
        public Guid? PromotionId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool? IsDeleted { get; set; }

        public MediaResponse? Media { get; set; }
        public ICollection<DiamondAccessoryResponse> DiamondAccessories { get; set; } = new List<DiamondAccessoryResponse>();
        public PromotionResponse? Promotion { get; set; }
        public ICollection<PaperworkResponse> PaperWorks { get; set; } = new List<PaperworkResponse>();
    }
}
