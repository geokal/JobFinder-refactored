using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InternshipApplied
    {
        [Key]
        public int Id { get; set; }
        public string CompanyEmailWhereStudentAppliedForInternship { get; set; }
        public string CompanyUniqueIDWhereStudentAppliedForInternship { get; set; }
        public string StudentEmailAppliedForInternship { get; set; }
        public string StudentUniqueIDAppliedForInternship { get; set; }
        public long RNGForInternshipApplied { get; set; }
        public string RNGForInternshipAppliedAsStudent_HashedAsUniqueID { get; set; }
        public DateTime DateTimeStudentAppliedForInternship { get; set; }

        public string InternshipStatusAppliedAtTheCompanySide { get; set; }
        public string InternshipStatusAppliedAtTheStudentSide { get; set; }

        public InternshipApplied_StudentDetails StudentDetails { get; set; }
        public InternshipApplied_CompanyDetails CompanyDetails { get; set; }
    }
}
