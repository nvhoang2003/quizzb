using System;
using System.Collections.Generic;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class UserCourse
{
    public int UserCoursesId { get; set; }

    public int UserId { get; set; }

    public int CoursesId { get; set; }

    public string Role { get; set; } = null!;

    public int? IsDeleted { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public virtual Course Courses { get; set; } = null!;

    public virtual ICollection<QuizUserAccess> QuizUserAccessAddByNavigations { get; set; } = new List<QuizUserAccess>();

    public virtual ICollection<QuizUserAccess> QuizUserAccessUsers { get; set; } = new List<QuizUserAccess>();

    public virtual User User { get; set; } = null!;
}
