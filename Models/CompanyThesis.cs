namespace QuizManager.Models
{
    public class CompanyThesis
    {
        // Thesis Details
        public int Id { get; set; }
        public long RNGForThesisUploadedAsCompany { get; set; }
        public string? RNGForThesisUploadedAsCompany_HashedAsUniqueID { get; set; }


        public string? CompanyEmailUsedToUploadThesis { get; set; }  // Foreign key to Company

        // Thesis Information
        public string? CompanyThesisTitle { get; set; }
        public string? CompanyThesisCompanySupervisorFullName { get; set; }
        public string? CompanyThesisDescriptionsUploaded { get; set; }
        public string? CompanyThesisAreasUpload { get; set; }
        public string? CompanyThesisSkillsNeeded { get; set; }
        public string? CompanyThesisStatus { get; set; }
        public string? CompanyThesisDepartment { get; set; }
        public string? CompanyThesisContactPersonEmail { get; set; }
        public string? CompanyThesisContactPersonTelephone { get; set; }
        public DateTime CompanyThesisUploadDateTime { get; set; }
        public DateTime CompanyThesisUpdateDateTime { get; set; }
        public DateTime CompanyThesisStartingDate { get; set; }
        public int CompanyThesisTimesUpdated { get; set; } = 0;
        public byte[]? CompanyThesisAttachmentUpload { get; set; }
        public ThesisType ThesisType { get; set; }

        // Professor Interest Information
        public bool IsProfessorInteresetedInCompanyThesis { get; set; }
        public string? IsProfessorInterestedInCompanyThesisStatus { get; set; }
        public string? ProfessorEmailInterestedInCompanyThesis { get; set; } // Foreign key to Professor

        // Navigation Properties
        public Company? Company { get; set; }
        public Professor? ProfessorInterested { get; set; }
    }
}
