namespace QuizManager.Models
{
    public class AnnouncementAsProfessor
    {
        public int Id { get; set; }
        public long? ProfessorAnnouncementRNG { get; set; }
        public string? ProfessorAnnouncementRNG_HashedAsUniqueID { get; set; }
        public string? ProfessorAnnouncementTitle { get; set; }
        public string? ProfessorAnnouncementDescription { get; set; }
        public string? ProfessorAnnouncementStatus { get; set; }
        public DateTime ProfessorAnnouncementUploadDate { get; set; }
        public string? ProfessorAnnouncementProfessorEmail { get; set; } // Foreign key to Professor
        public DateTime ProfessorAnnouncementTimeToBeActive { get; set; }
        public byte[]? ProfessorAnnouncementAttachmentFile { get; set; }

        // Navigation property to Professor
        public Professor? Professor { get; set; }
    }
}
