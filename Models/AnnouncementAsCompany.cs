namespace QuizManager.Models
{
    public class AnnouncementAsCompany
    {
        public int Id { get; set; }
        public long? CompanyAnnouncementRNG { get; set; }
        public string? CompanyAnnouncementRNG_HashedAsUniqueID { get; set; }
        public string? CompanyAnnouncementTitle { get; set; }
        public string? CompanyAnnouncementDescription { get; set; }
        public string? CompanyAnnouncementStatus { get; set; }
        public DateTime CompanyAnnouncementUploadDate { get; set; }
        public string? CompanyAnnouncementCompanyEmail { get; set; } // Foreign key to Company
        public DateTime CompanyAnnouncementTimeToBeActive { get; set; }
        public byte[]? CompanyAnnouncementAttachmentFile { get; set; }

        // Navigation property to Company
        public Company? Company { get; set; }
    }
}
