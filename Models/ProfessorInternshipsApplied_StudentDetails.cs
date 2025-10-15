using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class ProfessorInternshipsApplied_StudentDetails
    {
        [Key]
        public int Id { get; set; }
        public string StudentUniqueIDAppliedForProfessorInternship { get; set; }
        public string StudentEmailAppliedForProfessorInternship { get; set; }
        public DateTime DateTimeStudentAppliedForProfessorInternship { get; set; }
        public string RNGForProfessorInternshipAppliedAsStudent_HashedAsUniqueID { get; set; }
        public ProfessorInternshipApplied Application { get; set; }
    }
}
