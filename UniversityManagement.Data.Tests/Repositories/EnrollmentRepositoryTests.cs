// <copyright file="EnrollmentRepositoryTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Tests.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Data.Repositories;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests persistence operations for enrollments.
/// </summary>
public class EnrollmentRepositoryTests
{
    /// <summary>
    /// Verifies that adding an enrollment persists it and assigns an identifier.
    /// </summary>
    [Fact]
    public void Add_ShouldPersistEnrollment()
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
            var course = new Course("Math", "Mathematics", 5, 100, 500);
            var semester = new Semester(1, 30);
            var enrollment = new Enrollment(student, course, semester);

            var repository = new EnrollmentRepository(context);

            repository.Add(enrollment);

            Assert.True(enrollment.Id > 0);
            Assert.Equal(1, context.Enrollments.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an existing enrollment can be retrieved by identifier.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnStoredEnrollment()
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
            var course = new Course("Math", "Mathematics", 5, 100, 500);
            var semester = new Semester(1, 30);
            var enrollment = new Enrollment(student, course, semester);

            var repository = new EnrollmentRepository(context);
            repository.Add(enrollment);

            context.ChangeTracker.Clear();

            var storedEnrollment = repository.GetById(enrollment.Id);

            Assert.NotNull(storedEnrollment);
            Assert.Equal(enrollment.Id, storedEnrollment.Id);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an unknown identifier returns no enrollment.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnNullForUnknownId()
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

            var repository = new EnrollmentRepository(context);

            var enrollment = repository.GetById(999);

            Assert.Null(enrollment);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }
}
