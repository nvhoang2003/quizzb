using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuizBank : IAuditedEntityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string QuestionsType { get; set; } = null!;

    public string? GeneralFeedback { get; set; }

    public int CategoryId { get; set; }

    public sbyte? IsPublic { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public sbyte IsShuffle { get; set; }

    public int? AuthorId { get; set; }

    public float? DefaultMark { get; set; }

    public virtual User? Author { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<MatchSubQuestionBank> MatchSubQuestionBanks { get; set; } = new List<MatchSubQuestionBank>();

    public virtual ICollection<QbTag> QbTags { get; set; } = new List<QbTag>();

    public virtual ICollection<QuizbankAnswer> QuizbankAnswers { get; set; } = new List<QuizbankAnswer>();
}
