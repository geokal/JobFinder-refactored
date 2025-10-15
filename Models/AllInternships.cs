namespace QuizManager.Models
{
    public class AllInternships
    {
        public string? InternshipTitle { get; set; }
        public long RNGForCompanyInternship { get; set; }
        public long RNGForProfessorInternship { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyEmail { get; set; }
        public string? ProfessorName { get; set; }
        public string? ProfessorEmail { get; set; }
        public string? ProfessorSurname { get; set; }
        public string? ProfessorDepartment { get; set; }
        public string? ProfessorInternshipStatus { get; set; }
        public string? InternshipAreas { get; set; }
        public string? ProfessorInternshipAreas { get; set; }
        public string? InternshipType { get; set; }
        public string?  InternshipFundingType { get; set; }
        public DateTime InternshipActivePeriod { get; set; }
        public DateTime CompanyInternshipUploadDate { get; set; }
        public DateTime ProfessorInternshipUploadDate { get; set; }
        public DateTime InternshipFinishEstimation { get; set; }
        public string? InternshipStatus { get; set; }
        public bool InternshipTransportOffer { get; set; }
        public string? InternshipDimosLocation { get; set; }
        public string? InternshipPerifereiaLocation { get; set; }
        public string? InternshipDescription { get; set; }
        public string? InternshipTypeName { get; set; }

        public string? RNGForCompanyInternship_HashedAsUniqueID { get; set; }
        public string? RNGForProfessorInternship_HashedAsUniqueID { get; set; }

        public InternshipType ThesisType { get; set; } // Professor or Company thesis
    }

    public enum InternshipType
    {
        Professor,
        Company
    }


}
