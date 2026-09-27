namespace ReleaseChecklist.Api.Models
{
   
        public enum ReleaseStatus
        {
            PLANNED,
            ONGOING,
            DONE
        }

    public class Release
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset ReleaseDate { get; set; }
        public string? AdditionalInfo { get; set; }
        public List<string> CompletedStepIds { get; set; } = new();
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;

        // Pure computed property for status based on checked steps
        public ReleaseStatus Status
        {
            get;
            set;
        }
    }
}
