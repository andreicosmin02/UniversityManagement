// <copyright file="SemesterRepositoryTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Tests.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Data.Repositories;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests persistence operations for semesters.
/// </summary>
public class SemesterRepositoryTests
{
    /// <summary>
    /// Verifies that adding a semester persists it and assigns an identifier.
    /// </summary>
    [Fact]
    public void Add_ShouldPersistSemester()
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

            var repository = new SemesterRepository(context);
            var semester = new Semester(1, 30);

            repository.Add(semester);

            Assert.True(semester.Id > 0);
            Assert.Equal(1, context.Semesters.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an existing semester can be retrieved by identifier.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnStoredSemester()
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

            var repository = new SemesterRepository(context);
            var semester = new Semester(1, 30);

            repository.Add(semester);

            context.ChangeTracker.Clear();

            var storedSemester = repository.GetById(semester.Id);

            Assert.NotNull(storedSemester);
            Assert.Equal(1, storedSemester.Number);
            Assert.Equal(30, storedSemester.MinimumCredits);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an unknown identifier returns no semester.
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

            var repository = new SemesterRepository(context);

            var semester = repository.GetById(999);

            Assert.Null(semester);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }
}
