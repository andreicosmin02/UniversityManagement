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
}
