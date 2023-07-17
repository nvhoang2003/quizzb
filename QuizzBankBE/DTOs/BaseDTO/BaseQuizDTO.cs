using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.BaseDTO
{
    public abstract class BaseQuizDTO
    {

        public int Courseid { get; set; }

        [Required(ErrorMessage = "Please enter course name")]
        [StringLength(200, ErrorMessage = "Name length can't be more than 200.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(5000, ErrorMessage = "Description length can't be more than 5000.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please add TimeOpen to the request.")]
        [DataType(DataType.DateTime)]
        public DateTime? TimeOpen { get; set; }//ngay-thang-năm

        [Required(ErrorMessage = "Please add TimeClose to the request.")]
        [DataType(DataType.DateTime)]
        public DateTime? TimeClose { get; set; }//
        [Required]
        public string? TimeLimit { get; set; }//phút
        [Required]
        public string? Overduehanding { get; set; }//khi chưa lm xong đề thì hết h thì đây là đánh giá kết quả bài thi 
        [Required]
        public string? PreferedBehavior { get; set; }// cho cơ hội để luyện tập lại các câu hỏi đã sai 

        [Required(ErrorMessage ="Please enter Point to Pass")]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = ".0")]
        public float PointToPass { get; set; }//<max

        [Required(ErrorMessage = "Please enter Point to Pass")]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = ".0")]
        public float MaxPoint { get; set; }//>point

        [Required]
        public string NaveMethod { get; set; } = null!;//đưa ra action ...... ko thể sửa đáp án đã đưa ra 

        [Required]
        public sbyte IsPublic { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (TimeOpen.Value <= TimeClose.Value)
            {
                yield return new ValidationResult("Time Close must be greater than the Time Open.", new[] { "TimeClose" });
            }

            if (PointToPass > MaxPoint) {

                yield return new ValidationResult("PointToPass must be less than the MaxPoint.", new[] { "PointToPass" });
            }
        }
    }
}
