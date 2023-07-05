using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QbTag :IAuditedEntityBase
{
    public int Id { get; set; }

    public int? QbId { get; set; }

    public int? TagId { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public virtual QuizBank? Qb { get; set; }

    public virtual Tag? Tag { get; set; }
}
