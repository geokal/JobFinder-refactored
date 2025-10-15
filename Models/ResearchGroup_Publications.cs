using System.ComponentModel.DataAnnotations;

namespace QuizManager.Models
{
    public class ResearchGroup_Publications
    {
        [Key]
        public int Id { get; set; }
        public string PK_ResearchGroupEmail { get; set; }
        public string PK_ResearchGroupMemberEmail { get; set; }
        public string PK_ResearchGroupMemberPublication_Title { get; set; }
        public string PK_ResearchGroupMemberPublication_Authors { get; set; }
        public string PK_ResearchGroupMemberPublication_Journal { get; set; }
        public string PK_ResearchGroupMemberPublication_CitedBy { get; set; }
        public string PK_ResearchGroupMemberPublication_Year { get; set; }
        public string PK_ResearchGroupMemberPublication_Url { get; set; }


        public string MemberType { get; set; } = string.Empty; 

    }
}
