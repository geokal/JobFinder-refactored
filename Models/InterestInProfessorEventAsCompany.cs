using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
	public class InterestInProfessorEventAsCompany
	{
		[Key]
		public int Id { get; set; }
		public string ProfessorEmailWhereCompanyShowedInterest { get; set; }
		public string ProfessorUniqueIDWhereCompanyShowedInterest { get; set; }
		public string CompanyEmailShowInterestForProfessorEvent { get; set; }
		public string CompanyUniqueIDShowInterestForProfessorEvent { get; set; }
		public long RNGForProfessorEventInterestAsCompany { get; set; }
		public string RNGForProfessorEventInterestAsCompany_HashedAsUniqueID { get; set; }
		public DateTime DateTimeCompanyShowInterestForProfessorEvent { get; set; }
		public string? CompanyNumberOfPeopleToShowUpWhenShowInterestForProfessorEvent { get; set; }
		public string ProfessorEventStatus_ShowInterestAsCompany_AtProfessorSide { get; set; }
		public string ProfessorEventStatus_ShowInterestAsCompany_AtCompanySide { get; set; }

		public InterestInProfessorEventAsCompany_CompanyDetails CompanyDetails { get; set; }
		public InterestInProfessorEventAsCompany_ProfessorDetails ProfessorDetails { get; set; }
	}
}

