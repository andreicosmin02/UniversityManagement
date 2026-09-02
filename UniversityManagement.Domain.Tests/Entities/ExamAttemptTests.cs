// <copyright file="ExamAttemptTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Tests.Entities;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Unit tests for the <see cref="ExamAttempt"/> entity.
/// </summary>
public class ExamAttemptTests
{
    /// <summary>
    /// Verifies that an exam attempt stores its course, grade, and examination date.
    /// </summary>
    [Fact]
    public void ExamAttempt_ShouldStoreCourseGradeAndExamDate()
    {
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        var examDate = new DateTime(2026, 6, 15);

        var examAttempt = new ExamAttempt(
            course,
            8,
            examDate);

        Assert.Same(course, examAttempt.Course);
        Assert.Equal(8, examAttempt.Grade);
        Assert.Equal(examDate, examAttempt.ExamDate);
    }

    /// <summary>
    /// Verifies that an exam attempt cannot be created without a course.
    /// </summary>
    [Fact]
    public void ExamAttempt_ShouldRejectNullCourse()
    {
        var examDate = new DateTime(2026, 6, 15);

        Assert.Throws<ArgumentNullException>(
            () => new ExamAttempt(null!, 8, examDate));
    }

    /// <summary>
    /// Verifies that grades outside the valid range are rejected.
    /// </summary>
    /// <param name="grade">The grade to test.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(11)]
    public void ExamAttempt_ShouldRejectGradeOutsideValidRange(int grade)
    {
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        var examDate = new DateTime(2026, 6, 15);

        Assert.Throws<ArgumentException>(
            () => new ExamAttempt(course, grade, examDate));
    }

    /// <summary>
    /// Verifies that grades at the valid boundaries are accepted.
    /// </summary>
    /// <param name="grade">The grade to test.</param>
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void ExamAttempt_ShouldAcceptGradeAtValidBoundaries(int grade)
    {
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        var examDate = new DateTime(2026, 6, 15);

        var examAttempt = new ExamAttempt(
            course,
            grade,
            examDate);

        Assert.Equal(grade, examAttempt.Grade);
    }

    /// <summary>
    /// Verifies whether an exam attempt represents a passing grade.
    /// </summary>
    /// <param name="grade">The exam grade.</param>
    /// <param name="expected">The expected passing result.</param>
    [Theory]
    [InlineData(1, false)]
    [InlineData(4, false)]
    [InlineData(5, true)]
    [InlineData(10, true)]
    public void ExamAttempt_ShouldDetermineWhetherGradeIsPassing(
        int grade,
        bool expected)
    {
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        var examAttempt = new ExamAttempt(
            course,
            grade,
            new DateTime(2026, 6, 15));

        Assert.Equal(expected, examAttempt.Passed);
    }
}
