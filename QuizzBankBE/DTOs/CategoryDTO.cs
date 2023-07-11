
namespace QuizzBankBE.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? CreateBy { get; set; }

        public int? UpdateBy { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int IsDeleted { get; set; }
    }

    public class CreateCategoryDTO
    {
        public string Name { get; set; } = null!;

        public string? Description { get; set; }

    }
    public class CategoryResponseDTO
    {
        public string Status { get; set; }
       
        public int Version { get; set; }
        public int? IsDeleted { get; set; }
    }

}
