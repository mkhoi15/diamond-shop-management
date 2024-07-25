namespace DTO.DiamondAccessoryDto
{
    public class DiamondAccessoryResponse
    {
        public Guid Id { get; set; }
        public Guid DiamondId { get; set; }
        public string DiamondDetails { get; set; }
        public string? Origin { get; set; }
        public string? Color { get; set; }
        public string? Cut { get; set; }
        public string? Clarity { get; set; }
        public string? Weight { get; set; }
        public Guid AccessoryId { get; set; }
        public string AccessoryName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
