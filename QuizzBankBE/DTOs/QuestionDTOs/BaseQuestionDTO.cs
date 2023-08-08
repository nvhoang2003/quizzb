using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionDTOs
{
    public abstract class BaseQuestionDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string Name { get; set; }

        [Required]
        [StringLength(Const.MediumText)] //mediumtext 16 mib
        public string Content { get; set; }

        public string? Generalfeedback { get; set; }//phan hoi chung

        [IdExistValidation<User>("Id")]
        public int? AuthorId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Default Mark must be between 0 and 100.")]
        public float DefaultMark { get; set; }

        public sbyte IsShuffle { get; set; }
    }

    public class ListQuestion
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
    }
}
