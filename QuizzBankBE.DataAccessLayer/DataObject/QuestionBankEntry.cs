using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuestionBankEntry : IAuditedEntityBase
{
    public int IdquestionBankEntry { get; set; }

    public int QuestionCategoryId { get; set; }

    public int? IsDeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual QuestionCategory QuestionCategory { get; set; } = null!;

    public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; } = new List<QuestionQuiz>();

    public virtual ICollection<QuestionVersion> QuestionVersions { get; set; } = new List<QuestionVersion>();
}
