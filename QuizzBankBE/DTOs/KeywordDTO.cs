using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class KeywordDTO
    {

        public int Idkeywords { get; set; }
        [Required]
        public string Content { get; set; } = null!;

        public int CourseId { get; set; }

      
        public int? IsDeleted { get; set; }



    }

    public class CreateKeywordDTO
    {

        [Required]
        public string Content { get; set; } = null!;
    }

    public class KeywordResponseDTO
    {
        public string Status { get; set; }
       
        public int Version { get; set; }
        public int? IsDeleted { get; set; }
    }
}
