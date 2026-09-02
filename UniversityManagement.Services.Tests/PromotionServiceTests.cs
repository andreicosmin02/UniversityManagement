// <copyright file="PromotionServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests semester promotion operations.
/// </summary>
public class PromotionServiceTests
{
    /// <summary>
    /// Verifies that promotion is allowed when the minimum credit threshold is reached.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldReturnTrueWhenMinimumCreditsAreReached()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var semester = new Semester(1, 10);
        semester.AddCourse(courseA);
        semester.AddCourse(courseB);

        var attempts = new[]
        {
            new ExamAttempt(courseA, 5, new DateTime(2026, 6, 1)),
            new ExamAttempt(courseB, 7, new DateTime(2026, 6, 2)),
        };

        var service = new PromotionService();

        var result = service.CanPromote(semester, attempts);

        Assert.True(result);
    }

    /// <summary>
    /// Verifies that promotion is rejected when the minimum credit threshold is not reached.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldReturnFalseWhenMinimumCreditsAreNotReached()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var semester = new Semester(1, 10);
        semester.AddCourse(courseA);
        semester.AddCourse(courseB);

        var attempts = new[]
        {
            new ExamAttempt(courseA, 5, new DateTime(2026, 6, 1)),
            new ExamAttempt(courseB, 4, new DateTime(2026, 6, 2)),
        };

        var service = new PromotionService();

        var result = service.CanPromote(semester, attempts);

        Assert.False(result);
    }

    /// <summary>
    /// Verifies that failed courses do not contribute credits toward promotion.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldCountOnlyPassedCourses()
    {
        var passedCourse = new Course("A", "Course A", 6, 100m, 600m);
        var failedCourse = new Course("B", "Course B", 10, 100m, 1000m);

        var semester = new Semester(1, 10);
        semester.AddCourse(passedCourse);
        semester.AddCourse(failedCourse);

        var attempts = new[]
        {
            new ExamAttempt(passedCourse, 5, new DateTime(2026, 6, 1)),
            new ExamAttempt(failedCourse, 4, new DateTime(2026, 6, 2)),
        };

        var service = new PromotionService();

        Assert.False(service.CanPromote(semester, attempts));
    }
}
