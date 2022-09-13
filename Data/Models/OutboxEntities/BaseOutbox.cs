using Enums;

namespace Models.OutboxEntities
{
    public class BaseOutbox
    {
        public Guid Id { get; set; }
        public string Object { get; set; }
        public MethodType Method { get; set; }

        public bool IsCompleted { get; set; } = false;
        public DateTime CreatedDateTimeUtc { get; set; } = DateTime.UtcNow;
    }
}
