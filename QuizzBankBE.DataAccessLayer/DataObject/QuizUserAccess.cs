using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuizUserAccess : IAuditedEntityBase
{
    public int IdquizUserAccess { get; set; }

    public int UserId { get; set; }

    public int QuizId { get; set; }

    public int AddBy { get; set; }

    public DateTime? AddAt { get; set; }

    public int? IsDeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual UserCourse AddByNavigation { get; set; } = null!;

    public virtual Quiz Quiz { get; set; } = null!;

    public virtual UserCourse User { get; set; } = null!;
}
