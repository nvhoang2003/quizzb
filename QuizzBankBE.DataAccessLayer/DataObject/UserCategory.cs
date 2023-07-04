﻿using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class UserCategory : IAuditedEntityBase
{
    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public int Permission { get; set; }

    public int AddBy { get; set; }

    public int? IsDeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual User AddByNavigation { get; set; } = null!;

    public virtual QuestionCategory Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
