namespace QuizzBankBE.DTOs
{
    public class UserCategoryDTO
    {
        public int IdquestionCategories { get; set; }

        public string Name { get; set; } = null!;

        public int Parent { get; set; }

        public int? IsDeleted { get; set; }
    }
}
