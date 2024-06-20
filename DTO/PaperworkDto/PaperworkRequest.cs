using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace DTO.PaperworkDto
{
    public class PaperworkRequest
    {
        [Required]
        public string Type { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpirationDate { get; set; }

        public Guid? MediaId { get; set; }
    }
}
