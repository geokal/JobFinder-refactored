using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class CompanyJob
    {
        public int Id { get; set; }
        public DateTime UploadDateTime { get; set; }
        public string? EmailUsedToUploadJobs { get; set; } // This will be the foreign key to Company
        public string? PositionType { get; set; }
        public string? PositionTitle { get; set; }
        public string? PositionForeas { get; set; }
        public string? PositionContactPerson { get; set; }
        public string? PositionContactTelephonePerson { get; set; }
        public string? PositionAddressLocation { get; set; }
        public string? PositionPerifereiaLocation { get; set; }
        public string? PositionDimosLocation { get; set; }
        public string? PositionPostalCodeLocation { get; set; }
        public bool PositionTransportOffer { get; set; }
        public string? PositionAreas { get; set; }
        public DateTime PositionActivePeriod { get; set; }
        public string? PositionStatus { get; set; }
        public string? PositionDepartment { get; set; }

        [Required(ErrorMessage = "The Position Description field is required.")]
        [MaxLength(1000)]
        public string PositionDescription { get; set; } = string.Empty;
        public long RNGForPositionUploaded { get; set; }
        public byte[]? PositionAttachment { get; set; }
        public int TimesUpdated { get; set; } = 0;
        public DateTime UpdateDateTime { get; set; }
        public string? RNGForPositionUploaded_HashedAsUniqueID { get; set; }

        // Navigation property to Company Model 
        public Company? Company { get; set; }
    }
}

//SINEXIZW EDW APO AYRIO 15.07.25 STA MODELS TOU COMP[JOBS/THESIS/INTERNHIPS/EVENTS/ANNOUNCEMENTS] OLA 8A MEINOUN IDIA GIA TA POSITIONS APLA 8A FTIAXTEI NEO MODEL GIA COMPANY DETAILS GIA OLA
//POU 8A EXEI MONO TO EMAIL KAI TO UNIQUEID TIS ETAIREIA POU ANEVAZEI TO POSITION - TA IDIA META GIA PROFESSORS[THESIS/INTERNSHIPS/EVENTS/ANNOUNCEMENTS]