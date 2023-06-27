using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuestionQuiz
{
    public int QuestionId { get; set; }

    public int QuizzId { get; set; }

    public int? IsDeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual QuestionBankEntry Question { get; set; } = null!;

    public virtual Quiz Quizz { get; set; } = null!;
}
