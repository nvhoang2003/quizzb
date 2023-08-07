﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace QuizzBankBE.DataAccessLayer.DataObject;

public partial class QuizzbContext : DbContext
{
    public QuizzbContext()
    {
    }

    public QuizzbContext(DbContextOptions<QuizzbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<QuizBank> QuizBanks { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySQL("    server=103.161.178.66;port=3306;user=lmms;password=sa@123;database=quizzb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
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
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
