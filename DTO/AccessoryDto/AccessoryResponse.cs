using DTO.Media;

namespace DTO.AccessoryDto
{
    public class AccessoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid? MediaId { get; set; }
        public MediaResponse? Media { get; set; }
    }
}