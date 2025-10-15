using Microsoft.Extensions.Diagnostics.HealthChecks;
using Org.BouncyCastle.Asn1.Cms;
using System.ComponentModel.DataAnnotations.Schema; 


namespace QuizManager.Models
{
	public class CompanyEvent
	{
		public int Id { get; set; }
		public long RNGForEventUploadedAsCompany { get; set; }
		public string? CompanyEventType { get; set; } //list
		public bool CompanyEventOtherOrganizerToBeVisible { get; set; }
		public string? CompanyEventOtherOrganizer { get; set; }
		public string? CompanyEventAreasOfInterest { get; set; }
		public string? CompanyEventTitle { get; set; }
		public string? CompanyEventDescription { get; set; }
		public string? CompanyEventStatus { get; set; }
		public string? CompanyEventResponsiblePerson { get; set; }
		public string? CompanyEventResponsiblePersonEmail { get; set; }
		public string? CompanyEventResponsiblePersonTelephone { get; set; }
		public string? CompanyEventCompanyDepartment { get; set; }
		public DateTime CompanyEventUploadedDate { get; set; }
		public DateTime CompanyEventActiveDate { get; set; }
		public string? CompanyEventPerifereiaLocation { get; set; }
		public string? CompanyEventDimosLocation { get; set; }
		public string? CompanyEventPlaceLocation { get; set; }

		[NotMapped]
		public TimeOnly CompanyEventTimeOnly
		{
			get => TimeOnly.FromTimeSpan(CompanyEventTime);
			set => CompanyEventTime = value.ToTimeSpan();
		}

		public TimeSpan CompanyEventTime { get; set; }
		public string? CompanyEventPostalCodeLocation { get; set; }
		public byte[]? CompanyEventAttachmentFile { get; set; }
		public bool? CompanyEventOfferingTransportToEventLocation { get; set; }
		public string? CompanyEventStartingPointLocationToTransportPeopleToEvent1 { get; set; }
		public string? CompanyEventStartingPointLocationToTransportPeopleToEvent2 { get; set; }
		public string? CompanyEventStartingPointLocationToTransportPeopleToEvent3 { get; set; }
		public string? RNGForEventUploadedAsCompany_HashedAsUniqueID { get; set; }

		// Foreign key to Company
		public string? CompanyEmailUsedToUploadEvent { get; set; }

		// Navigation property to Company
		public Company? Company { get; set; }
	}
}
