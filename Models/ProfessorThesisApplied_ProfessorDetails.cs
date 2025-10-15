using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class ProfessorThesisApplied_ProfessorDetails
    {
        [Key]
        public int Id { get; set; }
        public string ProfessorUniqueIDWhereStudentAppliedForProfessorThesis { get; set; }
        public string ProfessorEmailWhereStudentAppliedForProfessorThesis { get; set; }
        public ProfessorThesisApplied Application { get; set; }
    }
}
