// <copyright file="CourseRepositoryTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Tests.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Data.Repositories;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests persistence operations for courses.
/// </summary>
public class CourseRepositoryTests
{
    /// <summary>
    /// Verifies that a course can be added to the repository.
    /// </summary>
    [Fact]
    public void Add_ShouldPersistCourse()
    {
        using var context = CreateContext();
        context.Database.EnsureCreated();

        try
        {
            var repository = new CourseRepository(context);
            var course = CreateCourse();

            repository.Add(course);

            Assert.True(course.Id > 0);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that a stored course can be retrieved by identifier.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnStoredCourse()
    {
        using var context = CreateContext();
        context.Database.EnsureCreated();

        try
        {
            var repository = new CourseRepository(context);
            var course = CreateCourse();
            repository.Add(course);

            var loadedCourse = repository.GetById(course.Id);

            Assert.NotNull(loadedCourse);
            Assert.Equal(course.Id, loadedCourse.Id);
            Assert.Equal(course.Name, loadedCourse.Name);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an unknown identifier returns no course.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnNullForUnknownId()
    {
        using var context = CreateContext();
        context.Database.EnsureCreated();

        try
        {
            var repository = new CourseRepository(context);

            var course = repository.GetById(int.MaxValue);

            Assert.Null(course);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that retrieving a course also loads its prerequisites.
    /// </summary>
    [Fact]
    public void GetById_ShouldLoadPrerequisites()
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

            var repository = new CourseRepository(context);
            repository.Add(course);

            context.ChangeTracker.Clear();

            var storedCourse = repository.GetById(course.Id);

            Assert.NotNull(storedCourse);

            var prerequisite = Assert.Single(storedCourse.Prerequisites);

            Assert.Equal(7, prerequisite.MinimumGrade);
            Assert.Equal("Programming", prerequisite.RequiredCourse.Name);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    private static UniversityManagementDbContext CreateContext()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var connectionString =
            $"Server=localhost;Database={databaseName};Integrated Security=True;TrustServerCertificate=True;";

        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new UniversityManagementDbContext(options);
    }

    private static Course CreateCourse()
    {
        return new Course(
            "Programming",
            "Introduction to programming.",
            5,
            100m,
            500m);
    }
}
