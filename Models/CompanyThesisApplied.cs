using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class CompanyThesisApplied
    {
        [Key]
        public int Id { get; set; }
        public string CompanyEmailWhereStudentAppliedForThesis { get; set; }
        public string CompanyUniqueIDWhereStudentAppliedForThesis { get; set; }
        public string StudentEmailAppliedForThesis { get; set; }
        public string StudentUniqueIDAppliedForThesis { get; set; }
        public long RNGForCompanyThesisApplied { get; set; }
        public string RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID { get; set; }
        public DateTime DateTimeStudentAppliedForThesis { get; set; }

        public string CompanyThesisStatusAppliedAtCompanySide { get; set; }
        public string CompanyThesisStatusAppliedAtStudentSide { get; set; }


        public CompanyThesisApplied_StudentDetails StudentDetails { get; set; }
        public CompanyThesisApplied_CompanyDetails CompanyDetails { get; set; }
    }

}
