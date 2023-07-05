using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class Quiz : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? CourseId { get; set; }

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

    public int? AuthorId { get; set; }

    public string? Description { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public virtual User? Author { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<QuizAccess> QuizAccesses { get; set; } = new List<QuizAccess>();

    public virtual ICollection<QuizQuestion> QuizQuestions { get; set; } = new List<QuizQuestion>();
}
