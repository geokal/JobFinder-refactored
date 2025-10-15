using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InternshipApplied_CompanyDetails
    {
        [Key]
        public int Id { get; set; }
        public string CompanyUniqueIDWhereStudentAppliedForInternship { get; set; }
        public string CompanyEmailWhereStudentAppliedForInternship { get; set; }
        public InternshipApplied Application { get; set; }
    }

}
