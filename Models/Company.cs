using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
	public class Company
	{
//STOIXEIA FOREA
		public int Id { get; set; }
        public string? CompanyEmail { get; set; }
        public string Company_UniqueID { get; set; }
        public byte[]? CompanyLogo { get; set; }
		public string? CompanyName { get; set; }
        public string? CompanyNameENG { get; set; }
        public string? CompanyShortName { get; set; }
        public string? CompanyType { get; set; }
        public string? CompanyActivity { get; set; }
        public long? CompanyTaxID { get; set; }
        public string? CompanyTaxOffice { get; set; }
        public string? CompanyTelephone { get; set; }
        public string? CompanyWebsite { get; set; }

		public string? CompanyPresentationEmbeddedVideo { get; set; }

		public string? CompanyWebsiteAnnouncements { get; set; }
        public string? CompanyWebsiteJobs { get; set; }
        public string? AtlasID { get; set; }
        public string? SvseID { get; set; }
        public DateTime? SvseDate { get; set; }

//STOIXEIA DIEYTHINSIS EDRAS
        public string? CompanyCountry { get; set; }
        public string? CompanyLocation { get; set; }
        public long? CompanyPC { get; set; }
        public string? CompanyRegions { get; set; }
        public string? CompanyTown { get; set; }

//TOMEAS DRASTIRIOTITAS
        public string? CompanyDescription { get; set; }
        public string? CompanyAreas { get; set; }
        public string? CompanyDesiredSkills { get; set; }
		/// ////////////////////////////////////////////////////////////////////////////////////////// DONE <summary>

//STOIXEIA IPEYTHINON DIAXEIRISIS PLATFORMAS
		public string? CompanyCEOName { get; set; }
        public string? CompanyCEOSurname { get; set; }
        public string? CompanyCEOTaxID { get; set; }
        public string? CompanyHRName { get; set; }
        public string? CompanyHRSurname { get; set; }
		public string? CompanyHREmail { get; set; }
        public string? CompanyHRTelephone { get; set; }
        public string? CompanyAdminName { get; set; }
        public string? CompanyAdminSurname { get; set; }
        public string? CompanyAdminEmail { get; set; }
        public string? CompanyAdminTelephone { get; set; }

 //STOIXEIA ETAIRIKIS DRASTIRIOTHTAS
        public int? CompanyEmployees { get; set; }
        public DateTime? CompanEmployeesLastUpdate { get; set; }
        public double? CompanyTurnover { get; set; }
        public DateTime? CompanyTurnoverLastUpdate { get; set; }
        public int? CompanyExportCountriesNumber { get; set; }
        public DateTime? CompanyExportCountriesNumberLastUpdate { get; set; }
        public string? CompanyExportCountries { get; set; }
        public DateTime? CompanyExportCountriesLastUpdate { get; set; }
        public bool CompanyVisibleActivity { get; set; }

//APODOHI SINENESIS
        public bool CompanyAcceptRules { get; set; }

//DIKA MOU APO PRIN TA KAINOURIA MIGRATIONS
		public string? CompanyDepartment { get; set; }
		public bool AskForExperience { get; set; }
	}

}
