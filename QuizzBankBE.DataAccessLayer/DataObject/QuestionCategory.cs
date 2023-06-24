﻿using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuestionCategory
{
    public int IdquestionCategories { get; set; }

    public string Name { get; set; } = null!;

    public int Parent { get; set; }

    public virtual User ParentNavigation { get; set; } = null!;

    public virtual ICollection<QuestionBankEntry> QuestionBankEntries { get; set; } = new List<QuestionBankEntry>();

    public virtual ICollection<UserCategory> UserCategories { get; set; } = new List<UserCategory>();
}
