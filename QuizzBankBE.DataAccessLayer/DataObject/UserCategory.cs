using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class UserCategory : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CategoryId { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? User { get; set; }
}
