using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AccessoryDto
{
	public class AccessoryResponse
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid? MediaId { get; set; }
        public string? MediaUrl { get; set; }
        public Guid? PromotionId { get; set; }
        public string? Discount { get; set; }
    }
}
