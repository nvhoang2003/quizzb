﻿using AutoMapper;
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

        public abstract string Questionstype { get; set; }

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
                        join qt in _dataContext.QbTags on q.Id equals qt.QuizBankId
                        join t in _dataContext.Tags on qt.TagId equals t.Id
                        where q.Id == questionBankID
                        select t).Distinct().ToList();

            Tags = _mapper.Map<List<TagDTO>>(tags);
        }
    }

    public abstract class BaseCreateQuestionBankDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string Name { get; set; }

        [Required]
        [StringLength(Const.MediumText)] //mediumtext 16 mib
        public string Content { get; set; }

        public abstract string Questionstype { get; set; }

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

        public enum QuestionType
        {
            MultiChoice,
            TrueFalse,
            Match,
            ShortAnswer,
            Numerical,
            DragAndDropIntoText,
        }

    }

    public class ListQuestionBank
    {
        public int Id { get; set; }

        public string Questionstype { get; set; }

        public string Name { get; set; }

        public string AuthorName { get; set; }

        public List<TagDTO> Tags { get; set; }

        public float DefaultMark { get; set; }

        public string CategoryName { get; set; }
    }

    public class GeneralQuestionBankDTO
    {
        public QuizBank Question { get; set; }
        public IEnumerable<QuizbankAnswer> Answers { get; set; }
        public IEnumerable<MatchSubQuestionBank> MatchAnswers { get; set; }
    }
}
