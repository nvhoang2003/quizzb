﻿using QuizzBankBE.DTOs.BaseDTO;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs.QuestionDTOs
{
    public class QuestionAnswerDTO : AnswerDTO
    {
        [Required]
        public string Content { get; set; }
        [Required]
        [Range(0, 1, ErrorMessage = "Fraction must be between 0% and 100%.")]
        public float Fraction { get; set; }
        public int? IsDeleted { get; set; }
        public int QuestionId { get; set; }
    }
}