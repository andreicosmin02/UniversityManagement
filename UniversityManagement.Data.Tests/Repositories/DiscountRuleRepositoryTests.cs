// <copyright file="DiscountRuleRepositoryTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Tests.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Data.Repositories;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests persistence operations for discount rules.
/// </summary>
public class DiscountRuleRepositoryTests
{
    /// <summary>
    /// Verifies that adding a discount rule persists it.
    /// </summary>
    [Fact]
    public void Add_ShouldPersistDiscountRule()
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
            var rule = new DiscountRule(new[] { course }, 10);
            var repository = new DiscountRuleRepository(context);

            repository.Add(rule);

            Assert.True(rule.Id > 0);
            Assert.Equal(1, context.DiscountRules.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that retrieving a discount rule also loads its courses.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnStoredDiscountRuleWithCourses()
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
            var rule = new DiscountRule(new[] { course }, 10);
            var repository = new DiscountRuleRepository(context);

            repository.Add(rule);
            context.ChangeTracker.Clear();

            var storedRule = repository.GetById(rule.Id);

            Assert.NotNull(storedRule);
            Assert.Equal(10, storedRule.Percentage);
            Assert.Single(storedRule.Courses);
            Assert.Equal("Math", storedRule.Courses.Single().Name);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an unknown identifier returns no discount rule.
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

            var repository = new DiscountRuleRepository(context);

            var rule = repository.GetById(999);

            Assert.Null(rule);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }
}
