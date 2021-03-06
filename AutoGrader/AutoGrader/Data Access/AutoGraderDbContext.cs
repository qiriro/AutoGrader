﻿using AutoGrader.Models;
using AutoGrader.Models.Assignment;
using AutoGrader.Models.Submission;
using AutoGrader.Models.Users;
using Microsoft.EntityFrameworkCore;
using AutoGrader.Models.ViewModels;

namespace AutoGrader.DataAccess
{
    public class AutoGraderDbContext : DbContext
    {
        public AutoGraderDbContext(DbContextOptions<AutoGraderDbContext> contextOptions) : base(contextOptions) { }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<SubmissionInput> SubmissionInputs { get; set; }
        public DbSet<SubmissionOutput> SubmissionOutputs { get; set; }
        public DbSet<TestCaseReport> TestCaseReports { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<TestCaseSpecification> TestCaseSpecifications { get; set; }
        public DbSet<StudentClass> StudentClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<StudentClass>()
                .HasKey(bc => new { bc.ClassId, bc.StudentId });
            modelBuilder.Entity<StudentClass>()
                .HasOne(bc => bc.Class)
                .WithMany(b => b.StudentsEnrolled)
                .HasForeignKey(bc => bc.ClassId);
            modelBuilder.Entity<StudentClass>()
                .HasOne(bc => bc.Student)
                .WithMany(c => c.StudentClasses)
                .HasForeignKey(bc => bc.StudentId);
        }

        public DbSet<AutoGrader.Models.ViewModels.JoinClassViewModel> JoinClassViewModel { get; set; }
    }
}
