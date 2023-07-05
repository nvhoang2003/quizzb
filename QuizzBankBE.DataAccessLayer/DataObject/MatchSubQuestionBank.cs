using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class MatchSubQuestionBank : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? QuestionBankId { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public string? QuestionText { get; set; }

    public string? AnswerText { get; set; }

    public virtual QuizBank? QuestionBank { get; set; }
}
