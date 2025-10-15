using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class CompanyThesisApplied_StudentDetails
    {
        [Key]
        public int Id { get; set; }
        public string StudentUniqueIDAppliedForThesis { get; set; }
        public string StudentEmailAppliedForThesis { get; set; }
        public DateTime DateTimeStudentAppliedForThesis { get; set; }
        public string RNGForCompanyThesisAppliedAsStudent_HashedAsUniqueID { get; set; }
        public CompanyThesisApplied Application { get; set; }
    }
}
