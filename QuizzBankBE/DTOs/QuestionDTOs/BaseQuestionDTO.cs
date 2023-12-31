﻿using QuizzBankBE.DataAccessLayer.DataObject;
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
        public float? DefaultMark { get; set; }

        public sbyte IsShuffle { get; set; }
    }

    public class ListQuestion
    {
        public int Id { get; set; }
        public string QuestionsType { get; set; }
        public string Name { get; set; }
        public string? Content { get; set; }
        public string AuthorName { get; set; }
        public List<TagDTO> Tags { get; set; }
        public float DefaultMark { get; set; }
        public string CategoryName { get; set; }
    }

    public class GeneralQuestionResultDTO : BaseQuestionDTO
    {
        public int Id { get; set; }
        public string Questionstype { get; set; }
    }

    public class GeneralQuestionDTO
    {
        public Question Question { get; set; }
        public IEnumerable<QuestionAnswer> Answers { get; set; }
        public IEnumerable<MatchSubQuestion> MatchAnswers { get; set; }
    }
}
