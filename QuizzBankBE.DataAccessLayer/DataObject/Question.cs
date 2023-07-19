using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class Question
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string QuestionsType { get; set; } = null!;

    public string? GeneralFeedback { get; set; }

    public int? AuthorId { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public sbyte IsShuffle { get; set; }

    public virtual User? Author { get; set; }

    public virtual ICollection<MatchSubQuestion> MatchSubQuestions { get; set; } = new List<MatchSubQuestion>();

    public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; } = new List<QuestionAnswer>();

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
