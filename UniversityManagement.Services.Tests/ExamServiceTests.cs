// <copyright file="ExamServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests exam operations.
/// </summary>
public class ExamServiceTests
{
    /// <summary>
    /// Verifies that a student cannot take an exam again after passing the course.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldRejectReexaminationAfterPassing()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var previousAttempts = new[]
        {
            new ExamAttempt(course, 5, new DateTime(2026, 6, 10)),
        };

        var service = new ExamService(3);

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterAttempt(
                course,
                8,
                new DateTime(2026, 7, 10),
                previousAttempts));
    }

    /// <summary>
    /// Verifies that another exam attempt is allowed after a failed attempt.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldAllowReexaminationAfterFailure()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var previousAttempts = new[]
        {
            new ExamAttempt(course, 4, new DateTime(2026, 6, 10)),
        };

        var service = new ExamService(3);

        var attempt = service.RegisterAttempt(
            course,
            7,
            new DateTime(2026, 7, 10),
            previousAttempts);

        Assert.Same(course, attempt.Course);
        Assert.Equal(7, attempt.Grade);
        Assert.Equal(new DateTime(2026, 7, 10), attempt.ExamDate);
    }

    /// <summary>
    /// Verifies that a first exam attempt is allowed.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldAllowFirstAttempt()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var service = new ExamService(3);

        var attempt = service.RegisterAttempt(
            course,
            6,
            new DateTime(2026, 6, 10),
            Array.Empty<ExamAttempt>());

        Assert.True(attempt.Passed);
    }

    /// <summary>
    /// Verifies that a student cannot take two exams on the same day.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldRejectSecondExamOnSameDay()
    {
        var firstCourse = new Course("A", "Course A", 5, 100m, 500m);
        var secondCourse = new Course("B", "Course B", 5, 100m, 500m);

        var previousAttempts = new[]
        {
            new ExamAttempt(firstCourse, 4, new DateTime(2026, 6, 10, 9, 0, 0)),
        };

        var service = new ExamService(3);

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterAttempt(
                secondCourse,
                7,
                new DateTime(2026, 6, 10, 15, 0, 0),
                previousAttempts));
    }

    /// <summary>
    /// Verifies that an exam can be taken on a different day.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldAllowExamOnDifferentDay()
    {
        var firstCourse = new Course("A", "Course A", 5, 100m, 500m);
        var secondCourse = new Course("B", "Course B", 5, 100m, 500m);

        var previousAttempts = new[]
        {
            new ExamAttempt(firstCourse, 4, new DateTime(2026, 6, 10)),
        };

        var service = new ExamService(3);

        var attempt = service.RegisterAttempt(
            secondCourse,
            7,
            new DateTime(2026, 6, 11),
            previousAttempts);

        Assert.Same(secondCourse, attempt.Course);
    }

    /// <summary>
    /// Verifies that a null exam history is rejected.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldRejectNullPreviousAttempts()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var service = new ExamService(3);

        Assert.Throws<ArgumentNullException>(
            () => service.RegisterAttempt(
                course,
                7,
                new DateTime(2026, 6, 10),
                null!));
    }

    /// <summary>
    /// Verifies that an exam is rejected when a prerequisite has not been passed.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldRejectMissingPrerequisite()
    {
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        course.AddPrerequisite(new Prerequisite(requiredCourse, 7));

        var service = new ExamService(3);

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterAttempt(
                course,
                8,
                new DateTime(2026, 7, 10),
                Array.Empty<ExamAttempt>()));
    }

    /// <summary>
    /// Verifies that passing a prerequisite below its required grade is insufficient.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldRejectPrerequisiteBelowRequiredGrade()
    {
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        course.AddPrerequisite(new Prerequisite(requiredCourse, 7));

        var previousAttempts = new[]
        {
            new ExamAttempt(requiredCourse, 6, new DateTime(2026, 6, 10)),
        };

        var service = new ExamService(3);

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterAttempt(
                course,
                8,
                new DateTime(2026, 7, 10),
                previousAttempts));
    }

    /// <summary>
    /// Verifies that an exam is allowed when all prerequisites are satisfied.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldAllowExamWhenPrerequisiteIsSatisfied()
    {
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        course.AddPrerequisite(new Prerequisite(requiredCourse, 7));

        var previousAttempts = new[]
        {
            new ExamAttempt(requiredCourse, 7, new DateTime(2026, 6, 10)),
        };

        var service = new ExamService(3);

        var attempt = service.RegisterAttempt(
            course,
            8,
            new DateTime(2026, 7, 10),
            previousAttempts);

        Assert.Same(course, attempt.Course);
    }

    /// <summary>
    /// Verifies that another attempt is rejected after reaching the maximum.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldRejectAttemptWhenMaximumIsReached()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var previousAttempts = new[]
        {
            new ExamAttempt(course, 2, new DateTime(2026, 6, 1)),
            new ExamAttempt(course, 3, new DateTime(2026, 6, 5)),
            new ExamAttempt(course, 4, new DateTime(2026, 6, 10)),
        };

        var service = new ExamService(3);

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterAttempt(
                course,
                4,
                new DateTime(2026, 6, 15),
                previousAttempts));
    }

    /// <summary>
    /// Verifies that an attempt is allowed before reaching the maximum.
    /// </summary>
    [Fact]
    public void RegisterAttempt_ShouldAllowAttemptBeforeMaximumIsReached()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var previousAttempts = new[]
        {
            new ExamAttempt(course, 2, new DateTime(2026, 6, 1)),
            new ExamAttempt(course, 4, new DateTime(2026, 6, 5)),
        };

        var service = new ExamService(3);

        var attempt = service.RegisterAttempt(
            course,
            4,
            new DateTime(2026, 6, 10),
            previousAttempts);

        Assert.Same(course, attempt.Course);
    }

    /// <summary>
    /// Verifies that the maximum number of attempts must be positive.
    /// </summary>
    /// <param name="maximumAttempts">The maximum number of attempts to test.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_ShouldRejectNonPositiveMaximumAttempts(int maximumAttempts)
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new ExamService(maximumAttempts));
    }
}
