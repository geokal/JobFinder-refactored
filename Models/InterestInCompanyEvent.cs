using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class InterestInCompanyEvent
    {
        [Key]
        public int Id { get; set; }
        public string CompanyEmailWhereStudentShowedInterest { get; set; }
        public string CompanyUniqueIDWhereStudentShowedInterest { get; set; }
        public string StudentEmailShowInterestForEvent { get; set; }
        public string StudentUniqueIDShowInterestForEvent { get; set; }
        public long RNGForCompanyEventInterest { get; set; }
        public string RNGForCompanyEventInterest_HashedAsUniqueID { get; set; }
        public DateTime DateTimeStudentShowInterest { get; set; }
        public string? StudentTransportNeedWhenShowInterestForCompanyEvent { get; set; } 
        public string? StudentTransportChosenLocationWhenShowInterestForCompanyEvent { get; set; } 
        public string CompanyEventStatusAtStudentSide { get; set; }
        public string CompanyEventStatusAtCompanySide { get; set; }

        // Navigation properties
        public InterestInCompanyEvent_StudentDetails StudentDetails { get; set; }
        public InterestInCompanyEvent_CompanyDetails CompanyDetails { get; set; }
    }
}


