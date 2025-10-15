using System.ComponentModel.DataAnnotations.Schema;

namespace QuizManager.Models
{
    public class ProfessorEvent
    {
        public int Id { get; set; }
        public long RNGForEventUploadedAsProfessor { get; set; }
        public string? ProfessorEventType { get; set; } //list
        public bool ProfessorEventOtherOrganizerToBeVisible { get; set; }
        public string? ProfessorEventOtherOrganizer { get; set; }
        public string? ProfessorEventAreasOfInterest { get; set; }
        public string? ProfessorEventTitle { get; set; }
        public string? ProfessorEventDescription { get; set; }
        public string? ProfessorEventStatus { get; set; }
        public string? ProfessorEventResponsiblePerson { get; set; }
        public string? ProfessorEventResponsiblePersonEmail { get; set; }
        public string? ProfessorEventResponsiblePersonTelephone { get; set; }
        public string? ProfessorEventUniversityDepartment { get; set; }
        public DateTime ProfessorEventUploadedDate { get; set; }
        public DateTime ProfessorEventActiveDate { get; set; }
        public string? ProfessorEventPerifereiaLocation { get; set; }
        public string? ProfessorEventDimosLocation { get; set; }
        public string? ProfessorEventPlaceLocation { get; set; }

        [NotMapped]
        public TimeOnly ProfessorEventTimeOnly
        {
            get => TimeOnly.FromTimeSpan(ProfessorEventTime);
            set => ProfessorEventTime = value.ToTimeSpan();
        }

        public TimeSpan ProfessorEventTime { get; set; }
        public string? ProfessorEventPostalCodeLocation { get; set; }
        public byte[]? ProfessorEventAttachmentFile { get; set; }
        public bool? ProfessorEventOfferingTransportToEventLocation { get; set; }
        public string? ProfessorEventStartingPointLocationToTransportPeopleToEvent1 { get; set; }
        public string? ProfessorEventStartingPointLocationToTransportPeopleToEvent2 { get; set; }
        public string? ProfessorEventStartingPointLocationToTransportPeopleToEvent3 { get; set; }
        public string? RNGForEventUploadedAsProfessor_HashedAsUniqueID { get; set; }

        // Foreign key to Professor
        public string? ProfessorEmailUsedToUploadEvent { get; set; }

        // Navigation property to Professor
        public Professor? Professor { get; set; }
    }
}
