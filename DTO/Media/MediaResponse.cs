namespace DTO.Media
{
    public class MediaResponse
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
