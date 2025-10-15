using Org.BouncyCastle.Crypto.Paddings;

namespace QuizManager.Models
{
	public class Professor
	{
		public int Id { get; set; }
        //PROSWPIKA STOIXEIA KATHIGITI
		public string? ProfEmail { get; set; }
		public string Professor_UniqueID { get; set; }
		public byte[]? ProfImage { get; set; }
        public string? ProfName { get; set; }
        public string? ProfSurname { get; set; }
        public string? ProfUniversity { get; set; }
        public string? ProfSchool { get; set; } //added 09/09/2025
        public string? ProfDepartment { get; set; }
        public string? ProfVahmidaDEP { get; set; }

        public string? ProfGnostikoAntikeimeno { get; set; } //latest
        public byte[]? ProfFEK { get; set; } //latest
        public string? ProfResearchGroup { get; set; } //latest

        //Ergastirio
        public string? ProfLab { get; set; } //latest
        public string? ProfLabFEK { get; set; } //latest
        public byte[]? ProfLabFEK_AttachmentFile { get; set; } //latest


        public string? ProfWorkTelephone { get; set; }
        public string? ProfPersonalTelephone { get; set; }
        public bool ProfPersonalTelephoneVisibility { get; set; }
        public string? ProfPersonalWebsite { get; set; }
        public string? ProfLinkedInSite { get; set; }
        public string? ProfScholarProfile { get; set; } 
        public string? ProfOrchidProfile { get; set; } 
        public string? ProfGeneralFieldOfWork { get; set; } 
        public string? ProfGeneralSkills { get; set; } 
        public string? ProfPersonalDescription { get; set; }
        public byte[]? ProfCVAttachment { get; set; } 
		public long? ProfRegistryNumber { get; set; } 
        public string? ProfCourses { get; set; } 

    }

}
