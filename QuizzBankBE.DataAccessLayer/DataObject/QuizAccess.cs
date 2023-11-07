using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuizAccess : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? QuizId { get; set; }

    public int? AddBy { get; set; }

    public DateTime? TimeStartQuiz { get; set; }

    public DateTime? TimeEndQuiz { get; set; }

    public string Status { get; set; } = null!;

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public virtual User? AddByNavigation { get; set; }

    public virtual Quiz? Quiz { get; set; }

    public virtual ICollection<QuizResponse> QuizResponses { get; set; } = new List<QuizResponse>();

    public virtual User? User { get; set; }
}
