﻿using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DTOs.QuestionBankDTOs;
using QuizzBankBE.DTOs.QuestionDTOs;
using QuizzBankBE.FormValidator;
using System.ComponentModel.DataAnnotations;

namespace QuizzBankBE.DTOs
{
    public class DoQuizResponseDTO
    {
        public int AccessId { get; set; }
        public float SumMark { get; set; }
        public float PointToPass { get; set; }
        public float MaxPoint { get; set; }
    }

    public abstract class DoQuestionDTO
    {
        [Required]
        [IdExistValidation<QuizAccess>("Id")]
        public int QuizAccessID { get; set; }
        [Required]
        [IdExistValidation<Question>("Id")]
        public int QuestionID { get; set; }
        public abstract string Questionstype { get; set; }
    }

    public class DoMatchingDTO : DoQuestionDTO
    {
        [Required]
        [RegularExpression("^Match$", ErrorMessage = "The Question Type must be equal to 'Match'")]
        public override string Questionstype { get; set; }
        public virtual List<MatchSubQuestionResponseDTO> MatchSubs { get; set; }
    }

    public class DoMultipleDTO : DoQuestionDTO
    {
        public DoMultipleDTO(int QuizAccessID, int QuestionID, List<int> AnswerIds)
        {
            this.QuizAccessID = QuizAccessID;
            this.QuestionID = QuestionID;
            this.Questionstype = "MultiChoice";
            this.AnswerId = AnswerIds;
        }
        [Required]
        [RegularExpression("^MultiChoice$", ErrorMessage = "The Question Type must be equal to 'MultiChoice'")]
        public override string Questionstype { get; set; }
        public List<int> AnswerId { get; set; }
        public List<object> AnswerSave { get; set; } = new List<object>();
    }

    public class DoTrueFalseDTO : DoQuestionDTO
    {
        [Required]
        [RegularExpression("^MultiChoice$", ErrorMessage = "The Question Type must be equal to 'MultiChoice'")]
        public override string Questionstype { get; set; }
        public DoTrueFalseAnswerDTO Answers { get; set; }
    }

    public class DoShortDTO : DoQuestionDTO
    {
        [Required]
        [RegularExpression("^ShortAnswer$", ErrorMessage = "The Question Type must be equal to 'ShortAnswer'")]
        public override string Questionstype { get; set; }
        public DoShortAnswerDTO Answers { get; set; }
    }
    public class DoDragDropTextDTO : DoQuestionDTO
    {
        [Required]
        [RegularExpression("^DragAndDropIntoText$", ErrorMessage = "The Question Type must be equal to 'DragAndDropIntoText'")]
        public override string Questionstype { get; set; }
        public List<DoDragDropChoiceDTO> Answers { get; set; }
    }

    public class DoMultipleAnswerDTO
    {
        [Required]
        [IdExistValidation<QuestionAnswer>("Id")]
        public int AnswerId { get; set; }
    }

    public class DoTrueFalseAnswerDTO
    {
        [Required]
        [IdExistValidation<QuestionAnswer>("Id")]
        public int AnswerId { get; set; }
    }

    public class DoShortAnswerDTO
    {
        [Required]
        [IdExistValidation<QuestionAnswer>("Id")]
        public int AnswerId { get; set; }
        [Required]
        public string Content { get; set; }
    }

    public class DoDragDropChoiceDTO
    {
        public int Position { get; set; }

        [Required]
        [IdExistValidation<QuestionAnswer>("Id")]
        public int AnswerId { get; set; }
    }
}
