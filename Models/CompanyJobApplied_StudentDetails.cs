using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class CompanyJobApplied_StudentDetails
    {
        [Key]
        public int Id { get; set; }
        public string StudentUniqueIDAppliedForCompanyJob { get; set; }
        public string StudentEmailAppliedForCompanyJob { get; set; }
        public DateTime DateTimeStudentAppliedForCompanyJob { get; set; }
        public string RNGForCompanyJobAppliedAsStudent_HashedAsUniqueID { get; set; }
        public CompanyJobApplied Application { get; set; }
    }
}
