using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuestionVersion
{
    public int IdquestionVersions { get; set; }

    public int QuestionId { get; set; }

    public int QuestionBankEntryId { get; set; }

    public string Status { get; set; } = null!;

    public int Version { get; set; }

    public int? IsDeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Question Question { get; set; } = null!;

    public virtual QuestionBankEntry QuestionBankEntry { get; set; } = null!;
}
