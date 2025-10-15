using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InternshipApplied_StudentDetails
    {
        [Key]
        public int Id { get; set; }
        public string StudentUniqueIDAppliedForInternship { get; set; }
        public string StudentEmailAppliedForInternship { get; set; }
        public DateTime DateTimeStudentAppliedForInternship { get; set; }
        public string RNGForInternshipAppliedAsStudent_HashedAsUniqueID { get; set; }
        public InternshipApplied Application { get; set; }
    }
}
