using AutoMapper;
using QuizzBankBE.DataAccessLayer.Data;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionBankDTOs
{
    public abstract class BaseQuestionBankDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string Name { get; set; }

        [Required]
        [StringLength(Const.MediumText)] //mediumtext 16 mib
        public string Content { get; set; }

        //public string Questionstype { get; set; }

        public string? Generalfeedback { get; set; }//phan hoi chung

        [Range(0, 1)]
        public sbyte? IsPublic { get; set; }

        [IdExistValidation<Category>("Id")]
        public int CategoryId { get; set; }

        public int? AuthorId { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Default Mark must be between 0 and 100.")]
        public float DefaultMark { get; set; }

        public sbyte IsShuffle { get; set; }

        public virtual ICollection<QbTagDTO> QbTags { get; set; }

        public List<TagDTO> Tags { get; set; }

        public enum QuestionType
        {
            MultiChoice,
            TrueFalse,
            Match,
            ShortAnswer,
            Numerical,
            DragAndDropIntoText,
        }

        public void addTags(int questionBankID, DataContext _dataContext, IMapper _mapper)
        {
            var tags = (from q in _dataContext.QuizBanks
                        join qt in _dataContext.QbTags on q.Id equals qt.QbId
                        join t in _dataContext.Tags on qt.TagId equals t.Id
                        where q.Id == questionBankID
                        select t).Distinct().ToList();

            Tags = _mapper.Map<List<TagDTO>>(tags);
        }
    }
}
