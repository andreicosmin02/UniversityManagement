// <copyright file="UniversityManagementDbContextTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Tests.Persistence;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests Entity Framework persistence.
/// </summary>
public class UniversityManagementDbContextTests
{
    /// <summary>
    /// Verifies that a course can be persisted and loaded from SQL Server.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistCourse()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var connectionString =
            $"Server=localhost;Database={databaseName};Integrated Security=True;TrustServerCertificate=True;";

        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        try
        {
            using (var context = new UniversityManagementDbContext(options))
            {
                context.Database.EnsureCreated();

                var course = new Course(
                    "Programming",
                    "Introduction to programming.",
                    5,
                    100m,
                    500m);

                context.Courses.Add(course);
                context.SaveChanges();

                Assert.True(course.Id > 0);
            }

            using (var context = new UniversityManagementDbContext(options))
            {
                var course = context.Courses.Single();

                Assert.Equal("Programming", course.Name);
                Assert.Equal(5, course.Credits);
                Assert.Equal(500m, course.Cost);
            }
        }
        finally
        {
            using var cleanupContext = new UniversityManagementDbContext(options);
            cleanupContext.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that saving a student persists it and assigns a database identifier.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistStudent()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                ["0722123456"],
                []);

            context.Students.Add(student);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            Assert.True(student.Id > 0);
            Assert.Equal(1, context.Students.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that a student's phone numbers are preserved by persistence.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistStudentPhoneNumbers()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                new[] { "0722123456" },
                Array.Empty<string>());

            context.Students.Add(student);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var storedStudent = context.Students.Single();

            Assert.Contains("0722123456", storedStudent.PhoneNumbers);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that a student's email addresses are preserved by persistence.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistStudentEmails()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                Array.Empty<string>(),
                new[] { "ion@example.com" });

            context.Students.Add(student);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var storedStudent = context.Students.Single();

            Assert.Contains("ion@example.com", storedStudent.Emails);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that saving a semester persists it and assigns a database identifier.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistSemester()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var semester = new Semester(1, 30);

            context.Semesters.Add(semester);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            Assert.True(semester.Id > 0);
            Assert.Equal(1, context.Semesters.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that a semester preserves its courses after persistence.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistSemesterCourses()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var course = new Course(
                "Mathematics",
                "Basic mathematics",
                5,
                100,
                500);

            var semester = new Semester(1, 30);
            semester.AddCourse(course);

            context.Semesters.Add(semester);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var storedSemester = context.Semesters
                .Include(item => item.Courses)
                .Single();

            Assert.Single(storedSemester.Courses);
            Assert.Equal("Mathematics", storedSemester.Courses.Single().Name);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that saving an enrollment persists it and assigns a database identifier.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistEnrollment()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                new[] { "0722123456" },
                Array.Empty<string>());

            var course = new Course(
                "Mathematics",
                "Basic mathematics",
                5,
                100,
                500);

            var semester = new Semester(1, 30);
            var enrollment = new Enrollment(student, course, semester);

            context.Enrollments.Add(enrollment);
            context.SaveChanges();

            Assert.True(enrollment.Id > 0);
            Assert.Equal(1, context.Enrollments.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an enrollment preserves its student, course, and semester.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistEnrollmentRelationships()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                new[] { "0722123456" },
                Array.Empty<string>());

            var course = new Course(
                "Mathematics",
                "Basic mathematics",
                5,
                100,
                500);

            var semester = new Semester(1, 30);
            var enrollment = new Enrollment(student, course, semester);

            context.Enrollments.Add(enrollment);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var storedEnrollment = context.Enrollments
                .Include(item => item.Student)
                .Include(item => item.Course)
                .Include(item => item.Semester)
                .Single();

            Assert.Equal("12345", storedEnrollment.Student.RegistrationNumber);
            Assert.Equal("Mathematics", storedEnrollment.Course.Name);
            Assert.Equal(1, storedEnrollment.Semester.Number);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that saving an exam attempt persists it and assigns a database identifier.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistExamAttempt()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var course = new Course(
                "Mathematics",
                "Basic mathematics",
                5,
                100,
                500);

            var attempt = new ExamAttempt(
                course,
                7,
                new DateTime(2026, 6, 10));

            context.ExamAttempts.Add(attempt);
            context.SaveChanges();

            Assert.True(attempt.Id > 0);
            Assert.Equal(1, context.ExamAttempts.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an exam attempt preserves its course after persistence.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistExamAttemptCourse()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var course = new Course(
                "Mathematics",
                "Basic mathematics",
                5,
                100,
                500);

            var attempt = new ExamAttempt(
                course,
                7,
                new DateTime(2026, 6, 10));

            context.ExamAttempts.Add(attempt);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var storedAttempt = context.ExamAttempts
                .Include(item => item.Course)
                .Single();

            Assert.Equal("Mathematics", storedAttempt.Course.Name);
            Assert.Equal(7, storedAttempt.Grade);
            Assert.True(storedAttempt.Passed);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that saving a payment transaction persists it and assigns a database identifier.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistPaymentTransaction()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                new[] { "0722123456" },
                Array.Empty<string>());

            var transaction = new PaymentTransaction(
                student,
                500m,
                new DateTime(2026, 6, 10));

            context.PaymentTransactions.Add(transaction);
            context.SaveChanges();

            Assert.True(transaction.Id > 0);
            Assert.Equal(1, context.PaymentTransactions.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that a payment transaction preserves its student and transaction data.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistPaymentTransactionRelationship()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                new[] { "0722123456" },
                Array.Empty<string>());

            var transaction = new PaymentTransaction(
                student,
                -200m,
                new DateTime(2026, 6, 10));

            context.PaymentTransactions.Add(transaction);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var storedTransaction = context.PaymentTransactions
                .Include(item => item.Student)
                .Single();

            Assert.Equal("12345", storedTransaction.Student.RegistrationNumber);
            Assert.Equal(-200m, storedTransaction.Amount);
            Assert.Equal(
                new DateTime(2026, 6, 10),
                storedTransaction.TransactionDate);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that a course preserves its prerequisites after persistence.
    /// </summary>
    [Fact]
    public void SaveChanges_ShouldPersistCoursePrerequisites()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var requiredCourse = new Course(
                "Programming",
                "Basic programming",
                5,
                100,
                500);

            var course = new Course(
                "Algorithms",
                "Algorithms and data structures",
                6,
                100,
                600);

            course.AddPrerequisite(
                new Prerequisite(requiredCourse, 7));

            context.Courses.Add(course);
            context.SaveChanges();

            context.ChangeTracker.Clear();

            var storedCourse = context.Courses
                .Include(item => item.Prerequisites)
                .ThenInclude(item => item.RequiredCourse)
                .Single(item => item.Name == "Algorithms");

            var prerequisite = Assert.Single(storedCourse.Prerequisites);

            Assert.Equal(7, prerequisite.MinimumGrade);
            Assert.Equal("Programming", prerequisite.RequiredCourse.Name);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }
}
