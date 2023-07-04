﻿using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class Quiz : IAuditedEntityBase
{
    public int Idquiz { get; set; }

    public int Courseid { get; set; }

    public string Name { get; set; } = null!;

    public string? Intro { get; set; }

    public DateTime? TimeOpen { get; set; }

    public DateTime? TimeClose { get; set; }

    public string? TimeLimit { get; set; }

    public string? Overduehanding { get; set; }

    public string? PreferedBehavior { get; set; }

    public float PointToPass { get; set; }

    public float MaxPoint { get; set; }

    public string NaveMethod { get; set; } = null!;

    public sbyte IsPublic { get; set; }

    public int? IsDeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; } = new List<QuestionQuiz>();

    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();

    public virtual ICollection<QuizUserAccess> QuizUserAccesses { get; set; } = new List<QuizUserAccess>();
}
