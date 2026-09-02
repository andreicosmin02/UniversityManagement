// <copyright file="CourseCostServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests course cost calculations.
/// </summary>
public class CourseCostServiceTests
{
    /// <summary>
    /// Verifies that course costs are summed when the discount combination is incomplete.
    /// </summary>
    [Fact]
    public void CalculateTotal_ShouldUseFullCostWhenDiscountCombinationIsIncomplete()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 600m);
        var rule = new DiscountRule(new[] { courseA, courseB }, 10m);

        var service = new CourseCostService();

        var total = service.CalculateTotal(new[] { courseA }, rule);

        Assert.Equal(500m, total);
    }

    /// <summary>
    /// Verifies that the configured discount is applied to every course in the combination.
    /// </summary>
    [Fact]
    public void CalculateTotal_ShouldApplyDiscountToEntireCombination()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 600m);
        var unrelatedCourse = new Course("C", "Course C", 5, 100m, 700m);

        var rule = new DiscountRule(new[] { courseA, courseB }, 10m);
        var service = new CourseCostService();

        var total = service.CalculateTotal(
            new[] { courseA, courseB, unrelatedCourse },
            rule);

        Assert.Equal(1690m, total);
    }
}
