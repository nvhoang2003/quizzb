using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class TagDTO
    {

        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

        public int? CategoryId { get; set; }

        public int? CreateBy { get; set; }

        public int? UpdateBy { get; set; }
        
        public DateTime? CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public int IsDeleted { get; set; }




    }

    public class CreateTagDTO
    {

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }

    }

    public class TagResponseDTO
    {
        public string Status { get; set; }
       
        public int Version { get; set; }
        public int? IsDeleted { get; set; }
    }
}
