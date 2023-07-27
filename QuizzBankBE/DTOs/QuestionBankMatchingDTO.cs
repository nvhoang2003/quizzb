﻿using Microsoft.AspNetCore.Http;
using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class CreateQuestionBankMatchingDTO : QuestionDTO
    {
        [RegularExpression("^Match$", ErrorMessage = "The Question Type must be equal to 'Match$'")]
        public string Questionstype { get; set; }

        public virtual List<MatchSubQuestionBankDTO> MatchSubs { get; set; }
    }
    public class QuestionBankMatchingResponseDTO : QuestionDTO
    {
        public int Id { get; set; }

        public MatchSubQuestionBankResponseDTO MatchSubs { get; set; }

        public string Questionstype { get; set; }
    }

    public class MatchSubQuestionBankDTO
    {
        [Required]
        [StringLength(Const.String)]
        public string QuestionText { get; set; }

        [Required]
        [StringLength(Const.String)]
        public string AnswerText { get; set; }
    }

    public class MatchSubQuestionBankResponseDTO
    {
        public List<string> QuestionTexts { get; set; } = null!;
        public List<string> AnswerTexts { get; set; } = null!;
    }
}
