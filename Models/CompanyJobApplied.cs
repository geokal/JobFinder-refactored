using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class CompanyJobApplied
    {
        [Key]
        public int Id { get; set; }
        public string CompanysEmailWhereStudentAppliedForCompanyJob { get; set; }
        public string CompanysUniqueIDWhereStudentAppliedForCompanyJob { get; set; }
        public string StudentEmailAppliedForCompanyJob { get; set; }
        public string StudentUniqueIDAppliedForCompanyJob { get; set; }
        public long RNGForCompanyJobApplied { get; set; }
        public string RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID { get; set; }
        public DateTime DateTimeStudentAppliedForCompanyJob { get; set; }

        public string CompanyPositionStatusAppliedAtTheCompanySide { get; set; }
        public string CompanyPositionStatusAppliedAtTheStudentSide { get; set; }

        public CompanyJobApplied_StudentDetails StudentDetails { get; set; }
        public CompanyJobApplied_CompanyDetails CompanyDetails { get; set; }
    }
}
