using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class Questionkeyword
{
    public int Questionid { get; set; }

    public int Keywordid { get; set; }

    public int? IsDeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Keyword Keyword { get; set; } = null!;

    public virtual Question Question { get; set; } = null!;
}
