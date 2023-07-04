using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class Keyword : IAuditedEntityBase
{
    public int Idkeywords { get; set; }

    public string Content { get; set; } = null!;

    public int CourseId { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public int? IsDeleted { get; set; }

    public virtual ICollection<Questionkeyword> Questionkeywords { get; set; } = new List<Questionkeyword>();
}
