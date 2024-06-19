namespace DTO.DiamondDto
{
    public class DiamondResponse
    {
        public string? Origin { get; set; }
        public string? Color { get; set; }
        public string? Cut { get; set; }
        public string? Clarity { get; set; }
        public string? Weight { get; set; }
        public decimal? Price { get; set; }
        public Guid? MediaId { get; set; }
        public bool? IsSold { get; set; }
        public Guid? PromotionId { get; set; }
        public string? MediaUrl { get; set; }
        public string? PromotionDescription { get; set; }
    }
}
