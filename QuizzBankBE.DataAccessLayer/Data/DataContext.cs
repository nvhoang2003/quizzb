using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using QuizzBankBE.DataAccessLayer.DataObject;
//using QuizzBankBE.DataAccessLayer.DataObject;
using QuizzBankBE.DataAccessLayer.Entity.Interface;
using System.Security.Claims;

namespace QuizzBankBE.DataAccessLayer.Data
{
    public partial class DataContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DataContext()
        {

        }

        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor)
       : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<MatchSubQuestion> MatchSubQuestions { get; set; }

        public virtual DbSet<MatchSubQuestionBank> MatchSubQuestionBanks { get; set; }

        public virtual DbSet<Permission> Permissions { get; set; }

        public virtual DbSet<QbTag> QbTags { get; set; }

        public virtual DbSet<Question> Questions { get; set; }

        public virtual DbSet<QuestionAnswer> QuestionAnswers { get; set; }

        public virtual DbSet<Quiz> Quizzes { get; set; }

        public virtual DbSet<QuizAccess> QuizAccesses { get; set; }

        public virtual DbSet<QuizBank> QuizBanks { get; set; }

        public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }

        public virtual DbSet<QuizResponse> QuizResponses { get; set; }

        public virtual DbSet<QuizbankAnswer> QuizbankAnswers { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<RolePermission> RolePermissions { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<User> Users { get; set; }

        public virtual DbSet<UserCategory> UserCategories { get; set; }

        public virtual DbSet<UserCourse> UserCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseMySQL("    server=103.161.178.66;port=3306;user=lmms;password=sa@123;database=quizzb");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("category");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("courses");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.EndDate)
                    .HasColumnType("datetime")
                    .HasColumnName("endDate");
                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .HasColumnName("fullName");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.ShortName)
                    .HasMaxLength(255)
                    .HasColumnName("shortName");
                entity.Property(e => e.StartDate)
                    .HasColumnType("datetime")
                    .HasColumnName("startDate");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");
            });

            modelBuilder.Entity<MatchSubQuestion>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("match_sub_questions");

                entity.HasIndex(e => e.QuestionId, "fk_matchsub_question_idx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AnswerText)
                    .HasMaxLength(255)
                    .HasColumnName("answerText");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.QuestionId).HasColumnName("questionId");
                entity.Property(e => e.QuestionText)
                    .HasMaxLength(255)
                    .HasColumnName("questionText");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");

                entity.HasOne(d => d.Question).WithMany(p => p.MatchSubQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("fk_matchsub_question");
            });

            modelBuilder.Entity<MatchSubQuestionBank>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("match_sub_question_banks");

                entity.HasIndex(e => e.QuestionBankId, "fk_matchsub_questionbank_idx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AnswerText)
                    .HasMaxLength(255)
                    .HasColumnName("answerText");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.QuestionBankId).HasColumnName("questionBankId");
                entity.Property(e => e.QuestionText)
                    .HasMaxLength(255)
                    .HasColumnName("questionText");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");

                entity.HasOne(d => d.QuestionBank).WithMany(p => p.MatchSubQuestionBanks)
                    .HasForeignKey(d => d.QuestionBankId)
                    .HasConstraintName("fk_matchsub_questionbank");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("permissions");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .HasColumnName("description");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");
            });

            modelBuilder.Entity<QbTag>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("qb_tags");

                entity.HasIndex(e => e.QbId, "fk_qb_tags_idx");

                entity.HasIndex(e => e.TagId, "fk_tag_qb_idx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.QbId).HasColumnName("qbId");
                entity.Property(e => e.TagId).HasColumnName("tagId");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");

                entity.HasOne(d => d.Qb).WithMany(p => p.QbTags)
                    .HasForeignKey(d => d.QbId)
                    .HasConstraintName("fk_qb_tags");

                entity.HasOne(d => d.Tag).WithMany(p => p.QbTags)
                    .HasForeignKey(d => d.TagId)
                    .HasConstraintName("fk_tag_qb");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("questions");

                entity.HasIndex(e => e.AuthorId, "fk_user_question_idx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AuthorId).HasColumnName("authorId");
                entity.Property(e => e.Content)
                    .HasColumnType("mediumtext")
                    .HasColumnName("content");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.DefaultMark).HasColumnName("defaultMark");
                entity.Property(e => e.GeneralFeedback)
                    .HasColumnType("mediumtext")
                    .HasColumnName("generalFeedback");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.IsShuffle).HasColumnName("isShuffle");
                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");
                entity.Property(e => e.QuestionsType)
                    .HasMaxLength(45)
                    .HasColumnName("questionsType");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");

                entity.HasOne(d => d.Author).WithMany(p => p.Questions)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("fk_user_question");
            });

            modelBuilder.Entity<QuestionAnswer>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("question_answer");

                entity.HasIndex(e => e.QuestionId, "fk_question_answer_idx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.Content)
                    .HasColumnType("mediumtext")
                    .HasColumnName("content");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.Feedback)
                    .HasMaxLength(255)
                    .HasColumnName("feedback");
                entity.Property(e => e.Fraction).HasColumnName("fraction");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.QuestionId).HasColumnName("questionId");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");

                entity.HasOne(d => d.Question).WithMany(p => p.QuestionAnswers)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("fk_question_answer");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("quiz");

                entity.HasIndex(e => e.AuthorId, "fk_author_quiz_idx");

                entity.HasIndex(e => e.CourseId, "fk_quiz_course_idx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AuthorId).HasColumnName("authorId");
                entity.Property(e => e.CourseId).HasColumnName("courseId");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.Description)
                    .HasColumnType("mediumtext")
                    .HasColumnName("description");
                entity.Property(e => e.Intro)
                    .HasColumnType("mediumtext")
                    .HasColumnName("intro");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.IsPublic)
                    .HasDefaultValueSql("'1'")
                    .HasColumnName("isPublic");
                entity.Property(e => e.MaxPoint).HasColumnName("maxPoint");
                entity.Property(e => e.Name)
                    .HasMaxLength(45)
                    .HasColumnName("name");
                entity.Property(e => e.NaveMethod)
                    .HasMaxLength(45)
                    .HasColumnName("naveMethod");
                entity.Property(e => e.Overduehanding)
                    .HasMaxLength(45)
                    .HasColumnName("overduehanding");
                entity.Property(e => e.PointToPass).HasColumnName("pointToPass");
                entity.Property(e => e.PreferedBehavior)
                    .HasMaxLength(45)
                    .HasColumnName("prefered_behavior");
                entity.Property(e => e.TimeClose)
                    .HasColumnType("datetime")
                    .HasColumnName("timeClose");
                entity.Property(e => e.TimeLimit)
                    .HasMaxLength(45)
                    .HasColumnName("timeLimit");
                entity.Property(e => e.TimeOpen)
                    .HasColumnType("datetime")
                    .HasColumnName("timeOpen");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");

                entity.HasOne(d => d.Author).WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("fk_author_quiz");

                entity.HasOne(d => d.Course).WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("fk_quiz_course");
            });

            modelBuilder.Entity<QuizAccess>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");

                entity.ToTable("quiz_accesses");

                entity.HasIndex(e => e.QuizId, "fk_quiz_access_idx");

                entity.HasIndex(e => e.UserId, "fk_user_access_idx");

                entity.HasIndex(e => e.AddBy, "fk_user_add_access_idx");

                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.AddBy).HasColumnName("addBy");
                entity.Property(e => e.CreateBy).HasColumnName("createBy");
                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");
                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                entity.Property(e => e.QuizId).HasColumnName("quizId");
                entity.Property(e => e.Status)
                    .HasMaxLength(45)
                    .HasDefaultValueSql("'wait'")
                    .HasColumnName("status");
                entity.Property(e => e.TimeStartQuiz)
                    .HasColumnType("datetime")
                    .HasColumnName("timeStartQuiz");
                entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                entity.Property(e => e.UpdateDate)
                    .HasColumnType("date")
                    .HasColumnName("updateDate");
                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.AddByNavigation).WithMany(p => p.QuizAccessAddByNavigations)
                    .HasForeignKey(d => d.AddBy)
                    .HasConstraintName("fk_user_add_access");

                entity.HasOne(d => d.Quiz).WithMany(p => p.QuizAccesses)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("fk_quiz_access");

                entity.HasOne(d => d.User).WithMany(p => p.QuizAccessUsers)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("fk_user_access");
            });

            modelBuilder.Entity<QuizBank>(entity =>
            {
                modelBuilder.Entity<QuizBank>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("quiz_banks");

                    entity.HasIndex(e => e.CategoryId, "fk_qb_category_idx");

                    entity.HasIndex(e => e.AuthorId, "fk_qb_user_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.AuthorId).HasColumnName("authorId");
                    entity.Property(e => e.CategoryId).HasColumnName("categoryId");
                    entity.Property(e => e.Content)
                        .HasColumnType("mediumtext")
                        .HasColumnName("content");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.DefaultMark).HasColumnName("defaultMark");
                    entity.Property(e => e.GeneralFeedback)
                        .HasColumnType("mediumtext")
                        .HasColumnName("generalFeedback");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.IsPublic)
                        .HasDefaultValueSql("'1'")
                        .HasColumnName("isPublic");
                    entity.Property(e => e.IsShuffle).HasColumnName("isShuffle");
                    entity.Property(e => e.Name)
                        .HasMaxLength(45)
                        .HasColumnName("name");
                    entity.Property(e => e.QuestionsType)
                        .HasMaxLength(45)
                        .HasColumnName("questionsType");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");

                    entity.HasOne(d => d.Author).WithMany(p => p.QuizBanks)
                        .HasForeignKey(d => d.AuthorId)
                        .HasConstraintName("fk_qb_user");

                    entity.HasOne(d => d.Category).WithMany(p => p.QuizBanks)
                        .HasForeignKey(d => d.CategoryId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_qb_category");
                });

                modelBuilder.Entity<QuizQuestion>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("quiz_questions");

                    entity.HasIndex(e => e.QuestionId, "fk_question_quiz_idx");

                    entity.HasIndex(e => e.QuizzId, "fk_quiz_question_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.QuestionId).HasColumnName("question_id");
                    entity.Property(e => e.QuizzId).HasColumnName("quizz_id");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");

                    entity.HasOne(d => d.Question).WithMany(p => p.QuizQuestions)
                        .HasForeignKey(d => d.QuestionId)
                        .HasConstraintName("fk_question_quiz");

                    entity.HasOne(d => d.Quizz).WithMany(p => p.QuizQuestions)
                        .HasForeignKey(d => d.QuizzId)
                        .HasConstraintName("fk_quiz_question");
                });

                modelBuilder.Entity<QuizResponse>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("quiz_responses");

                    entity.HasIndex(e => e.AccessId, "fk_access_idx");

                    entity.HasIndex(e => e.QuestionId, "fk_response_question_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.AccessId).HasColumnName("accessId");
                    entity.Property(e => e.Answer)
                        .HasColumnType("json")
                        .HasColumnName("answer");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.Mark).HasColumnName("mark");
                    entity.Property(e => e.QuestionId).HasColumnName("questionId");
                    entity.Property(e => e.Status)
                        .HasMaxLength(45)
                        .HasDefaultValueSql("'pass'")
                        .HasColumnName("status");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");

                    entity.HasOne(d => d.Access).WithMany(p => p.QuizResponses)
                        .HasForeignKey(d => d.AccessId)
                        .HasConstraintName("fk_access");

                    entity.HasOne(d => d.Question).WithMany(p => p.QuizResponses)
                        .HasForeignKey(d => d.QuestionId)
                        .HasConstraintName("fk_response_question");
                });

                modelBuilder.Entity<QuizbankAnswer>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("quizbank_answers");

                    entity.HasIndex(e => e.QuizBankId, "fk_quizbank_answer_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.Content)
                        .HasColumnType("mediumtext")
                        .HasColumnName("content");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.Feedback)
                        .HasMaxLength(255)
                        .HasColumnName("feedback");
                    entity.Property(e => e.Fraction).HasColumnName("fraction");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.QuizBankId).HasColumnName("quizBankId");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");

                    entity.HasOne(d => d.QuizBank).WithMany(p => p.QuizbankAnswers)
                        .HasForeignKey(d => d.QuizBankId)
                        .HasConstraintName("fk_quizbank_answer");
                });

                modelBuilder.Entity<Role>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("role");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.Description)
                        .HasMaxLength(255)
                        .HasColumnName("description");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.Name)
                        .HasMaxLength(255)
                        .HasColumnName("name");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");
                });

                modelBuilder.Entity<RolePermission>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("role_permissions");

                    entity.HasIndex(e => e.PermissionId, "fk_permission_role_idx");

                    entity.HasIndex(e => e.RoleId, "fk_role_permission_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.PermissionId).HasColumnName("permissionID");
                    entity.Property(e => e.RoleId).HasColumnName("roleID");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");

                    entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                        .HasForeignKey(d => d.PermissionId)
                        .HasConstraintName("fk_permission_role");

                    entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                        .HasForeignKey(d => d.RoleId)
                        .HasConstraintName("fk_role_permission");
                });

                modelBuilder.Entity<Tag>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("tags");

                    entity.HasIndex(e => e.CategoryId, "fk_tag_category_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.CategoryId).HasColumnName("categoryId");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.Description)
                        .HasColumnType("text")
                        .HasColumnName("description");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.Name)
                        .HasMaxLength(45)
                        .HasColumnName("name");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");

                    entity.HasOne(d => d.Category).WithMany(p => p.Tags)
                        .HasForeignKey(d => d.CategoryId)
                        .HasConstraintName("fk_tag_category");
                });

                modelBuilder.Entity<User>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("users");

                    entity.HasIndex(e => e.RoleId, "fk_user_role_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.Address)
                        .HasMaxLength(255)
                        .HasColumnName("address");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.Dob)
                        .HasColumnType("datetime")
                        .HasColumnName("dob");
                    entity.Property(e => e.Email)
                        .HasMaxLength(255)
                        .HasColumnName("email");
                    entity.Property(e => e.FirstName)
                        .HasMaxLength(255)
                        .HasColumnName("firstName");
                    entity.Property(e => e.Gender).HasColumnName("gender");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.LastName)
                        .HasMaxLength(255)
                        .HasColumnName("lastName");
                    entity.Property(e => e.Password)
                        .HasMaxLength(255)
                        .HasColumnName("password");
                    entity.Property(e => e.Phone)
                        .HasMaxLength(10)
                        .HasColumnName("phone");
                    entity.Property(e => e.RoleId).HasColumnName("roleId");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");
                    entity.Property(e => e.UserName)
                        .HasMaxLength(255)
                        .HasColumnName("userName");

                    entity.HasOne(d => d.Role).WithMany(p => p.Users)
                        .HasForeignKey(d => d.RoleId)
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("fk_user_role");
                });

                modelBuilder.Entity<UserCategory>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("user_categories");

                    entity.HasIndex(e => e.UserId, "fk_category_user_idx");

                    entity.HasIndex(e => e.CategoryId, "fk_user_category_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.CategoryId).HasColumnName("categoryId");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");
                    entity.Property(e => e.UserId).HasColumnName("userId");

                    entity.HasOne(d => d.Category).WithMany(p => p.UserCategories)
                        .HasForeignKey(d => d.CategoryId)
                        .HasConstraintName("fk_user_category");

                    entity.HasOne(d => d.User).WithMany(p => p.UserCategories)
                        .HasForeignKey(d => d.UserId)
                        .HasConstraintName("fk_category_user");
                });

                modelBuilder.Entity<UserCourse>(entity =>
                {
                    entity.HasKey(e => e.Id).HasName("PRIMARY");

                    entity.ToTable("user_courses");

                    entity.HasIndex(e => e.CoursesId, "fk_couse_user_idx");

                    entity.HasIndex(e => e.UserId, "fk_user_courses_idx");

                    entity.Property(e => e.Id).HasColumnName("ID");
                    entity.Property(e => e.CoursesId).HasColumnName("coursesId");
                    entity.Property(e => e.CreateBy).HasColumnName("createBy");
                    entity.Property(e => e.CreateDate)
                        .HasColumnType("date")
                        .HasColumnName("createDate");
                    entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");
                    entity.Property(e => e.Role)
                        .HasMaxLength(45)
                        .HasColumnName("role");
                    entity.Property(e => e.UpdateBy).HasColumnName("updateBy");
                    entity.Property(e => e.UpdateDate)
                        .HasColumnType("date")
                        .HasColumnName("updateDate");
                    entity.Property(e => e.UserId).HasColumnName("userId");

                    entity.HasOne(d => d.Courses).WithMany(p => p.UserCourses)
                        .HasForeignKey(d => d.CoursesId)
                        .HasConstraintName("fk_couse_user");

                    entity.HasOne(d => d.User).WithMany(p => p.UserCourses)
                        .HasForeignKey(d => d.UserId)
                        .HasConstraintName("fk_user_course");
                });

                OnModelCreatingPartial(modelBuilder);
            });
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default(CancellationToken))
        {
            var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is IAuditedEntityBase && (
            e.State == EntityState.Added
            || e.State == EntityState.Modified));
            ;

            var check = _httpContextAccessor?.HttpContext?.User?.Claims
    .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var isUserLoggedIn = int.TryParse(check, out var userId);

            var modifiedOrCreatedBy = !isUserLoggedIn ? 0 : int.Parse(check);

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((IAuditedEntityBase)entityEntry.Entity).CreateDate = DateTime.Now;
                    ((IAuditedEntityBase)entityEntry.Entity).CreateBy = modifiedOrCreatedBy;
                }
                else
                {
                    Entry((IAuditedEntityBase)entityEntry.Entity).Property(p => p.CreateDate).IsModified = false;
                    Entry((IAuditedEntityBase)entityEntry.Entity).Property(p => p.CreateBy).IsModified = false;
                }

                ((IAuditedEntityBase)entityEntry.Entity).UpdateDate = DateTime.Now;
                ((IAuditedEntityBase)entityEntry.Entity).UpdateBy = modifiedOrCreatedBy;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
