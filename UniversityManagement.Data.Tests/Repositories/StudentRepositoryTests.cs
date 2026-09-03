// <copyright file="StudentRepositoryTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Tests.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Data.Repositories;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests persistence operations for students.
/// </summary>
public class StudentRepositoryTests
{
    /// <summary>
    /// Verifies that adding a student persists it and assigns an identifier.
    /// </summary>
    [Fact]
    public void Add_ShouldPersistStudent()
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

            var repository = new StudentRepository(context);
            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                new[] { "0722123456" },
                Array.Empty<string>());

            repository.Add(student);

            Assert.True(student.Id > 0);
            Assert.Equal(1, context.Students.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an existing student can be retrieved by identifier.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnStoredStudent()
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

            var repository = new StudentRepository(context);
            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                new[] { "0722123456" },
                new[] { "ion@example.com" });

            repository.Add(student);

            context.ChangeTracker.Clear();

            var storedStudent = repository.GetById(student.Id);

            Assert.NotNull(storedStudent);
            Assert.Equal(student.Id, storedStudent.Id);
            Assert.Equal("12345", storedStudent.RegistrationNumber);
            Assert.Contains("0722123456", storedStudent.PhoneNumbers);
            Assert.Contains("ion@example.com", storedStudent.Emails);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an unknown identifier returns no student.
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

            var repository = new StudentRepository(context);

            var student = repository.GetById(999);

            Assert.Null(student);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }
}
