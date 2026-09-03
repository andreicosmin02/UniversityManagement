// <copyright file="ExamAttemptRepositoryTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Tests.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Data.Repositories;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests persistence operations for exam attempts.
/// </summary>
public class ExamAttemptRepositoryTests
{
    /// <summary>
    /// Verifies that adding an exam attempt persists it and assigns an identifier.
    /// </summary>
    [Fact]
    public void Add_ShouldPersistExamAttempt()
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

            var course = new Course("Math", "Mathematics", 5, 100, 500);
            var attempt = new ExamAttempt(
                course,
                7,
                new DateTime(2026, 6, 10));

            var repository = new ExamAttemptRepository(context);

            repository.Add(attempt);

            Assert.True(attempt.Id > 0);
            Assert.Equal(1, context.ExamAttempts.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an existing exam attempt can be retrieved by identifier.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnStoredExamAttempt()
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

            var course = new Course("Math", "Mathematics", 5, 100, 500);
            var attempt = new ExamAttempt(
                course,
                7,
                new DateTime(2026, 6, 10));

            var repository = new ExamAttemptRepository(context);
            repository.Add(attempt);

            context.ChangeTracker.Clear();

            var storedAttempt = repository.GetById(attempt.Id);

            Assert.NotNull(storedAttempt);
            Assert.Equal(7, storedAttempt.Grade);
            Assert.Equal(new DateTime(2026, 6, 10), storedAttempt.ExamDate);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an unknown identifier returns no exam attempt.
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

            var repository = new ExamAttemptRepository(context);

            var attempt = repository.GetById(999);

            Assert.Null(attempt);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }
}
