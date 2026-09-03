// <copyright file="DiscountRuleTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Tests.Entities;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests the <see cref="DiscountRule"/> entity.
/// </summary>
public class DiscountRuleTests
{
    /// <summary>
    /// Verifies that a discount rule stores its courses and percentage.
    /// </summary>
    [Fact]
    public void DiscountRule_ShouldStoreCoursesAndPercentage()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 600m);

        var rule = new DiscountRule(new[] { courseA, courseB }, 10m);

        Assert.Equal(2, rule.Courses.Count);
        Assert.Equal(10m, rule.Percentage);
    }

    /// <summary>
    /// Verifies that an invalid discount percentage is rejected.
    /// </summary>
    /// <param name="percentage">The discount percentage to test.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(101)]
    public void DiscountRule_ShouldRejectInvalidPercentage(decimal percentage)
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new DiscountRule(new[] { course }, percentage));
    }

    /// <summary>
    /// Verifies that a new discount rule starts without a persistent identifier.
    /// </summary>
    [Fact]
    public void DiscountRule_ShouldStartWithZeroId()
    {
        var course = new Course("Math", "Mathematics", 5, 100, 500);

        var rule = new DiscountRule(new[] { course }, 10);

        Assert.Equal(0, rule.Id);
    }

    /// <summary>
    /// Verifies that an existing discount rule can store a persistent identifier.
    /// </summary>
    [Fact]
    public void DiscountRule_ShouldStorePositiveId()
    {
        var course = new Course("Math", "Mathematics", 5, 100, 500);

        var rule = new DiscountRule(1, new[] { course }, 10);

        Assert.Equal(1, rule.Id);
    }

    /// <summary>
    /// Verifies that an existing discount rule rejects a non-positive identifier.
    /// </summary>
    /// <param name="id">The invalid identifier to test.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void DiscountRule_ShouldRejectNonPositiveId(int id)
    {
        var course = new Course("Math", "Mathematics", 5, 100, 500);

        Assert.Throws<ArgumentException>(
            () => new DiscountRule(id, new[] { course }, 10));
    }

    /// <summary>
    /// Verifies that a discount rule rejects a null course collection.
    /// </summary>
    [Fact]
    public void DiscountRule_ShouldRejectNullCourses()
    {
        Assert.Throws<ArgumentNullException>(
            () => new DiscountRule(null!, 10m));
    }

    /// <summary>
    /// Verifies that a discount rule rejects an empty course combination.
    /// </summary>
    [Fact]
    public void DiscountRule_ShouldRejectEmptyCourseCombination()
    {
        Assert.Throws<ArgumentException>(
            () => new DiscountRule(Array.Empty<Course>(), 10m));
    }

    /// <summary>
    /// Verifies that a discount rule rejects a null course in its combination.
    /// </summary>
    [Fact]
    public void DiscountRule_ShouldRejectNullCourse()
    {
        Assert.Throws<ArgumentException>(
            () => new DiscountRule(new Course[] { null! }, 10m));
    }

    /// <summary>
    /// Verifies that a discount rule rejects duplicate courses.
    /// </summary>
    [Fact]
    public void DiscountRule_ShouldRejectDuplicateCourse()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);

        Assert.Throws<ArgumentException>(
            () => new DiscountRule(new[] { course, course }, 10m));
    }
}
