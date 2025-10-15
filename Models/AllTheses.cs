namespace QuizManager.Models
{
    public class AllTheses
    {
        public string? ThesisTitle { get; set; }
        public long RNGForCompanyThesisUploaded { get; set; }
        public long RNGForProfessorThesisUploaded { get; set; }
        public string? CompanyNameUploadedThesis { get; set; }
        public string? CompanyThesisAreasUpload { get; set; }
        public string? ProfessorThesisAreasUpload { get; set; }
        public string? EmailUsedToUploadThesis { get; set; }
        public string? ProfessorName { get; set; }
        public string? ProfessorSurname { get; set; }
        public string? ProfessorDepartment { get; set; }
        public DateTime ThesisUploadDateTime { get; set; }
        public string? ProfessorThesisStatus { get; set; }
        public string? CompanyThesisStatus { get; set; }

        public string? RNGForCompanyThesisUploaded_HashedAsUniqueID { get; set; }
        public string? RNGForProfessorThesisUploaded_HashedAsUniqueID { get; set; }


        public ThesisType ThesisType { get; set; } // Professor or Company thesis
    }

    public enum ThesisType
    {
        Professor,
        Company
    }
}
