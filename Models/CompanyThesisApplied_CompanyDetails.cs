using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class CompanyThesisApplied_CompanyDetails
    {
        [Key]
        public int Id { get; set; }
        public string CompanyUniqueIDWhereStudentAppliedForThesis { get; set; }
        public string CompanyEmailWhereStudentAppliedForThesis { get; set; }
        public CompanyThesisApplied Application { get; set; }
    }
}
