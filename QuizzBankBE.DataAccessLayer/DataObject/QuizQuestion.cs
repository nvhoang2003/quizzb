using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuizQuestion : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? QuizzId { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public virtual Question? Question { get; set; }

    public virtual Quiz? Quizz { get; set; }
}
