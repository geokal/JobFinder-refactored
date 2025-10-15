namespace QuizManager.Models
{
    public class PlatformActions
    {
        public int Id { get; set; }
        public string? UserRole_PerformedAction { get; set; }
        public string? ForWhat_PerformedAction { get; set; }
        public string? HashedPositionRNG_PerformedAction { get; set; }
        public string? TypeOfAction_PerformedAction { get; set; }
        public DateTime DateTime_PerformedAction { get; set; }
    }
}
