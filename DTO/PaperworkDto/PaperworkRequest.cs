using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTO.PaperworkDto
{
    public class PaperworkRequest
    {
        [Required(ErrorMessage = "Please enter a type Certificate or Warranty")]
        public string? Type { get; set; }

        [Required(ErrorMessage = "diamond id is required")]
        public Guid DiamondId { get; set; }

        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Please select a date")]
        [DataType(DataType.DateTime)]
        public DateTime ExpirationDate { get; set; }

        public string? Status { get; set; }
    }
}
