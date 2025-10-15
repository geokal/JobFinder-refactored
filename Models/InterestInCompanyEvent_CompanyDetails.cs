using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InterestInCompanyEvent_CompanyDetails
    {
        [Key]
        public int Id { get; set; }
        public string CompanyUniqueIDWhereStudentShowInterestForCompanyEvent { get; set; }
        public string CompanyEmailWhereStudentShowInterestForCompanyEvent { get; set; }
        public InterestInCompanyEvent Application { get; set; }
    }

}
