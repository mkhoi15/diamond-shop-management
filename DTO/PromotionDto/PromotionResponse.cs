using DTO.DiamondDto;

namespace DTO.PromotionDto
{
    public class PromotionResponse
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Discount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsActive { get; set; }

        public ICollection<DiamondResponse> Diamonds { get; set; } = new List<DiamondResponse>();
    }
}
