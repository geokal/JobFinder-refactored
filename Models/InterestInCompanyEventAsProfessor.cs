using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
	public class InterestInCompanyEventAsProfessor
	{
		[Key]
		public int Id { get; set; }
		public string CompanyEmailWhereProfessorShowedInterest { get; set; }
		public string CompanyUniqueIDWhereProfessorShowedInterest { get; set; }
		public string ProfessorEmailShowInterestForCompanyEvent { get; set; }
		public string ProfessorUniqueIDShowInterestForCompanyEvent { get; set; }
		public long RNGForCompanyEventInterestAsProfessor { get; set; }
		public string RNGForCompanyEventInterestAsProfessor_HashedAsUniqueID { get; set; }
		public DateTime DateTimeProfessorShowInterestForCompanyEvent { get; set; }
		public string CompanyEventStatus_ShowInterestAsProfessor_AtCompanySide { get; set; }
		public string CompanyEventStatus_ShowInterestAsProfessor_AtProfessorSide { get; set; }

		public InterestInCompanyEventAsProfessor_CompanyDetails CompanyDetails { get; set; }
		public InterestInCompanyEventAsProfessor_ProfessorDetails ProfessorDetails { get; set; }
	}
}
