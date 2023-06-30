using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class QuestionCategoryDTO
    {
        public int IdquestionCategories { get; set; }

        public string Name { get; set; }
        public int? IsDeleted { get; set; }
        public int Parent { get; set; }
        public virtual ICollection<QuestionBankEntryResponseDTO> QuestionBankEntries { get; set; }
    }

    public class CreateQuestionCategoryDTO
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        public int Parent { get; set; }
    }
}
