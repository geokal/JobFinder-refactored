using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class ProfessorThesisApplied_StudentDetails
    {
        [Key]
        public int Id { get; set; }
        public string StudentUniqueIDAppliedForProfessorThesis { get; set; }
        public string StudentEmailAppliedForProfessorThesis { get; set; }
        public DateTime DateTimeStudentAppliedForProfessorThesis { get; set; }
        public string RNGForProfessorThesisApplied_HashedAsUniqueID { get; set; }
        public ProfessorThesisApplied Application { get; set; }
    }
}
