namespace QuizManager.Models
{
    public class ThesisWithProfessor
    {
        public int ThesisId { get; set; }
        public string? ThesisTitle { get; set; }
        public string? ThesisDescription { get; set; }
        public DateTime UploadDateTime { get; set; }
        public string? ProfessorName { get; set; }
        public string? ProfessorSurnname { get; set; }
        public string? ProfessorEmail { get; set; }
        public byte[]? ThesisAttachment { get; set; }
        public long RNGForThesisUploaded { get; set; }
    }

}
