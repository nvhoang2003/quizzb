using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class UserCourse : IAuditedEntityBase
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CoursesId { get; set; }

    public int? CreateBy { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int IsDeleted { get; set; }

    public virtual Course? Courses { get; set; }

    public virtual User? User { get; set; }
}
