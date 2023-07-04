﻿using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class Question : IAuditedEntityBase
{
    public int Idquestions { get; set; }

    public string Name { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Questionstype { get; set; } = null!;

    public string? Generalfeedback { get; set; }

    public int Createdby { get; set; }

    public int Updatedby { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public float DefaultMark { get; set; }

    public int? IsDeleted { get; set; }

    public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();

    public virtual User CreatedbyNavigation { get; set; } = null!;

    public virtual ICollection<QuestionVersion> QuestionVersions { get; set; } = new List<QuestionVersion>();

    public virtual ICollection<Questionkeyword> Questionkeywords { get; set; } = new List<Questionkeyword>();

    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();

    public virtual User UpdatedbyNavigation { get; set; } = null!;
}
