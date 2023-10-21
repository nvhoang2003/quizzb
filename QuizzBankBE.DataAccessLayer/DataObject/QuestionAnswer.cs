using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuestionAnswer : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public string Content { get; set; } = null!;

    public float Fraction { get; set; }

    public string? Feedback { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public int? FileId { get; set; }

    public virtual SystemFile? SystemFile { get; set; }

    public virtual Question? Question { get; set; }
}
