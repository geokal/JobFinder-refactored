namespace QuizManager.Models
{
    public class JobWithCompany
    {
        public int JobId { get; set; }
        public string PositionTitle { get; set; }
        public string PositionType { get; set; }
        public string PositionDescription { get; set; }
        public DateTime UploadDateTime { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDptApplied { get; set; }
        public string CompanyEmail { get; set; }    //latest
        public byte[]? PositionAttachment { get; set; }

        public long RNGForPositionUploaded { get; set; } // This needs to be fetched


    }


}
