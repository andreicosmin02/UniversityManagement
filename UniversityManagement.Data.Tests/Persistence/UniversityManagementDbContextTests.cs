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
}
