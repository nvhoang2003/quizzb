using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuizResponse : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? AccessId { get; set; }

    public int? Mark { get; set; }

    public string? Status { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public int? QuestionId { get; set; }

    public string? Answer { get; set; }

    public virtual QuizAccess? Access { get; set; }

    public virtual Question? Question { get; set; }
}
