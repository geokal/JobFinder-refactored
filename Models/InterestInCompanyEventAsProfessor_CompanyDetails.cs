using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
	public class InterestInCompanyEventAsProfessor_CompanyDetails
	{
		[Key]
		public int Id { get; set; }
		public string CompanyUniqueIDWhereProfessorShowInterestForCompanyEvent { get; set; }
		public string CompanyEmailWhereProfessorShowInterestForCompanyEvent { get; set; }
		public InterestInCompanyEventAsProfessor EventInterest { get; set; }
	}
}
