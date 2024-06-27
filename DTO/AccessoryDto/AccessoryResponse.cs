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
        public string PromotionName { get; set; }
        public string MediaUrl { get; set; }
    }
}