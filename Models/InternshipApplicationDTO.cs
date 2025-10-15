namespace QuizManager.Models
{
    public class InternshipApplicationDTO
    {
        public string? InternshipTitle { get; set; }
        public string? StudentName { get; set; }
        public string? StudentSurname { get; set; }
        public long StudentRegNumber { get; set; }
        public string? StudentEmail { get; set; }
        public string? InternshipStatus { get; set; }
        public DateTime DateTimeStudentApplied { get; set; }
        public string? CompanyOrProfessorName { get; set; }
        public string? CompanyOrProfessorEmail { get; set; }
        public string? InternshipStatusAppliedAtTheCompanyByStudent { get; set; }
        public string?  InternshipStatusAppliedAtTheCompanyByCompany { get; set; }
        public string? CompanyAppliedForInternship { get; set; }
        public long RNGForInternshipApplied { get; set; }
        public string? InternsnipTitleAppliedAtTheCompany { get; set; }
        public DateTime DateTimeStudentAppliedForInternship { get; set; }
        public string? CompanyEmailAppliedForInternship { get; set; }
    }
}
