﻿using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionBankDTOs
{
    public class QuestionBankShortAnswerDTO : BaseQuestionBankDTO
    {
        public int Id { get; set; }
        public List<QuestionBankAnswerDTO> Answers { get; set; }

        [RegularExpression("^ShortAnswer$", ErrorMessage = "The Question Type must be equal to 'ShortAnswer'")]
        public string Questionstype { get; set; }

        public List<Tag> Tags { get; set; }
    }

    public class CreateQuestionBankShortAnswerDTO : BaseQuestionBankDTO
    {
        public string Questionstype { get; set; }
        public List<QuestionBankAnswerDTO> Answers { get; set; }
    }
}