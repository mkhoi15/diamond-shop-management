using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PaperworkDto
{
    public class PaperworkResponse
    {
        public Guid Id { get; set; }
        public Guid DiamondId { get; set; }
        public string? Type { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? Status { get; set; }
        public string? MediaUrl { get; set; }
    }
}
