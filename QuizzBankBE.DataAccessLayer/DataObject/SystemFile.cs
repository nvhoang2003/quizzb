using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class SystemFile : IAuditedEntityBase
{
    public int Id { get; set; }

    public string NameFile { get; set; } = null!;

    public string PathFile { get; set; } = null!;

    public string TypeFile { get; set; } = null!;

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public virtual ICollection<MatchSubQuestionBank> MatchSubQuestionBanks { get; set; } = new List<MatchSubQuestionBank>();

    public virtual ICollection<MatchSubQuestion> MatchSubQuestions { get; set; } = new List<MatchSubQuestion>();

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual ICollection<QuizBank> QuizBanks { get; set; } = new List<QuizBank>();

    public virtual ICollection<QuizbankAnswer> QuizbankAnswers { get; set; } = new List<QuizbankAnswer>();
}
