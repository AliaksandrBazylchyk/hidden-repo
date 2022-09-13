namespace Models.Entities
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public int Year { get; set; }
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
    }
}