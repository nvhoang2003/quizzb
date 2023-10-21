using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QbTag : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? QuizBankId { get; set; }

    public int? TagId { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public int? QuestionId { get; set; }

    public virtual Question? Question { get; set; }

    public virtual QuizBank? QuizBank { get; set; }

    public virtual Tag? Tag { get; set; }
}
