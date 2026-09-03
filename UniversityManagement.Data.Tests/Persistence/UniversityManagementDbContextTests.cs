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
}
