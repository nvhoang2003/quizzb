﻿using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class MatchSubQuestion : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? QuestionId { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public string? QuestionText { get; set; }

    public string? AnswerText { get; set; }

    public int? FileId { get; set; }

    public virtual SystemFile? SystemFile { get; set; }

    public virtual Question? Question { get; set; }
}
