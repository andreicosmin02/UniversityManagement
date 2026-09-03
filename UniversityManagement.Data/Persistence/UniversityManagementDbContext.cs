// <copyright file="UniversityManagementDbContext.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Persistence;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Represents the Entity Framework database context for university management data.
/// </summary>
public class UniversityManagementDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UniversityManagementDbContext"/> class.
    /// </summary>
    /// <param name="options">The options used to configure the database context.</param>
    public UniversityManagementDbContext(
        DbContextOptions<UniversityManagementDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the courses stored in the database.
    /// </summary>
    public DbSet<Course> Courses => this.Set<Course>();

    /// <summary>
    /// Gets the students stored in the database.
    /// </summary>
    public DbSet<Student> Students => this.Set<Student>();

    /// <summary>
    /// Gets the semesters stored in the database.
    /// </summary>
    public DbSet<Semester> Semesters => this.Set<Semester>();

    /// <summary>
    /// Gets the enrollments stored in the database.
    /// </summary>
    public DbSet<Enrollment> Enrollments => this.Set<Enrollment>();

    /// <summary>
    /// Gets the exam attempts stored in the database.
    /// </summary>
    public DbSet<ExamAttempt> ExamAttempts => this.Set<ExamAttempt>();

    /// <summary>
    /// Gets the payment transactions stored in the database.
    /// </summary>
    public DbSet<PaymentTransaction> PaymentTransactions =>
        this.Set<PaymentTransaction>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(course => course.Id);

            entity.Property(course => course.Name);
            entity.Property(course => course.Description);
            entity.Property(course => course.Credits);
            entity.Property(course => course.MinimumCostPerCredit);
            entity.Property(course => course.Cost);

            entity.Ignore(course => course.Prerequisites);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(student => student.Id);

            entity.Property(student => student.Id)
                .ValueGeneratedOnAdd();

            entity.Property(student => student.FirstName)
                .IsRequired();

            entity.Property(student => student.LastName)
                .IsRequired();

            entity.Property(student => student.Address)
                .IsRequired();

            entity.Property(student => student.Cnp)
                .IsRequired();

            entity.Property(student => student.RegistrationNumber)
                .IsRequired();

            entity.PrimitiveCollection(student => student.PhoneNumbers)
                .HasField("phoneNumbers");

            entity.PrimitiveCollection(student => student.Emails)
                .HasField("emails");
        });

        modelBuilder.Entity<Semester>(entity =>
        {
            entity.HasKey(semester => semester.Id);

            entity.Property(semester => semester.Id)
                .ValueGeneratedOnAdd();

            entity.Property(semester => semester.Number)
                .IsRequired();

            entity.Property(semester => semester.MinimumCredits)
                .IsRequired();

            entity.HasMany(semester => semester.Courses)
                .WithMany();

            entity.Ignore(semester => semester.TotalAvailableCredits);
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(enrollment => enrollment.Id);

            entity.Property(enrollment => enrollment.Id)
                .ValueGeneratedOnAdd();

            entity.HasOne(enrollment => enrollment.Student)
                .WithMany()
                .IsRequired();

            entity.HasOne(enrollment => enrollment.Course)
                .WithMany()
                .IsRequired();

            entity.HasOne(enrollment => enrollment.Semester)
                .WithMany()
                .IsRequired();
        });

        modelBuilder.Entity<ExamAttempt>(entity =>
        {
            entity.HasKey(attempt => attempt.Id);

            entity.Property(attempt => attempt.Id)
                .ValueGeneratedOnAdd();

            entity.Property(attempt => attempt.Grade)
                .IsRequired();

            entity.Property(attempt => attempt.ExamDate)
                .IsRequired();

            entity.HasOne(attempt => attempt.Course)
                .WithMany()
                .IsRequired();

            entity.Ignore(attempt => attempt.Passed);
        });

        modelBuilder.Entity<PaymentTransaction>(entity =>
        {
            entity.HasKey(transaction => transaction.Id);

            entity.Property(transaction => transaction.Id)
                .ValueGeneratedOnAdd();

            entity.Property(transaction => transaction.Amount)
                .IsRequired();

            entity.Property(transaction => transaction.TransactionDate)
                .IsRequired();

            entity.HasOne(transaction => transaction.Student)
                .WithMany()
                .IsRequired();
        });
    }
}
