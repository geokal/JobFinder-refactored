using Org.BouncyCastle.Crypto.Paddings;

namespace QuizManager.Models
{
    public class ProfessorThesis
    {
        // Thesis Details
        public int Id { get; set; }
        public long RNGForThesisUploaded { get; set; }
        public string? RNGForThesisUploaded_HashedAsUniqueID { get; set; }

        // Foreign key to Professor
        public string? ProfessorEmailUsedToUploadThesis { get; set; }

        // Thesis Information
        public string? ThesisTitle { get; set; }
        public string? ThesisDescription { get; set; }
        public string? ThesisAreas { get; set; }
        public string? ThesisSkills { get; set; }
        public byte[]? ThesisAttachment { get; set; }
        public DateTime ThesisUploadDateTime { get; set; }
        public DateTime ThesisActivePeriod { get; set; }
        public DateTime ThesisUpdateDateTime { get; set; }
        public string? ThesisStatus { get; set; }
        public int ThesisTimesUpdated { get; set; } = 0;
        public ThesisType ThesisType { get; set; }

        // Company Interest Information
        public bool IsCompanyInteresetedInProfessorThesis { get; set; }
        public string? IsCompanyInterestedInProfessorThesisStatus { get; set; }
        public string? CompanyEmailInterestedInProfessorThesis { get; set; } // Foreign key to Company

        // Navigation Properties
        public Professor? Professor { get; set; }
        public Company? CompanyInterested { get; set; }
    }
}
