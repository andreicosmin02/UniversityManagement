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
    /// Verifies that an exam attempt stores its student, course, grade, and examination date.
    /// </summary>
    [Fact]
    public void ExamAttempt_ShouldStoreCourseGradeAndExamDate()
    {
        var student = CreateStudent();
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        var examDate = new DateTime(2026, 6, 15);

        var examAttempt = new ExamAttempt(
            student,
            course,
            8,
            examDate);

        Assert.Same(student, examAttempt.Student);
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
        var student = CreateStudent();
        var examDate = new DateTime(2026, 6, 15);

        Assert.Throws<ArgumentNullException>(
            () => new ExamAttempt(
                student,
                null!,
                8,
                examDate));
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
        var student = CreateStudent();
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        var examDate = new DateTime(2026, 6, 15);

        Assert.Throws<ArgumentException>(
            () => new ExamAttempt(
                student,
                course,
                grade,
                examDate));
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
        var student = CreateStudent();
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        var examDate = new DateTime(2026, 6, 15);

        var examAttempt = new ExamAttempt(
            student,
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
        var student = CreateStudent();
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        var examAttempt = new ExamAttempt(
            student,
            course,
            grade,
            new DateTime(2026, 6, 15));

        Assert.Equal(expected, examAttempt.Passed);
    }

    /// <summary>
    /// Verifies that a new exam attempt starts without a persistent identifier.
    /// </summary>
    [Fact]
    public void ExamAttempt_ShouldStartWithZeroId()
    {
        var student = CreateStudent();
        var course = new Course("Math", "Mathematics", 5, 100m, 500m);

        var attempt = new ExamAttempt(
            student,
            course,
            7,
            new DateTime(2026, 6, 10));

        Assert.Equal(0, attempt.Id);
    }

    /// <summary>
    /// Verifies that an existing exam attempt can store a persistent identifier.
    /// </summary>
    [Fact]
    public void ExamAttempt_ShouldStorePositiveId()
    {
        var student = CreateStudent();
        var course = new Course("Math", "Mathematics", 5, 100m, 500m);

        var attempt = new ExamAttempt(
            1,
            student,
            course,
            7,
            new DateTime(2026, 6, 10));

        Assert.Equal(1, attempt.Id);
    }

    /// <summary>
    /// Verifies that an existing exam attempt rejects a non-positive identifier.
    /// </summary>
    /// <param name="id">The invalid identifier to test.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ExamAttempt_ShouldRejectNonPositiveId(int id)
    {
        var student = CreateStudent();
        var course = new Course("Math", "Mathematics", 5, 100m, 500m);

        Assert.Throws<ArgumentException>(
            () => new ExamAttempt(
                id,
                student,
                course,
                7,
                new DateTime(2026, 6, 10)));
    }

    /// <summary>
    /// Verifies that an exam attempt stores the student who took the exam.
    /// </summary>
    [Fact]
    public void ExamAttempt_ShouldStoreStudent()
    {
        var student = CreateStudent();
        var course = new Course("Math", "Mathematics", 5, 100m, 500m);

        var attempt = new ExamAttempt(
            student,
            course,
            7,
            new DateTime(2026, 6, 10));

        Assert.Same(student, attempt.Student);
    }

    /// <summary>
    /// Verifies that an exam attempt cannot be created without a student.
    /// </summary>
    [Fact]
    public void ExamAttempt_ShouldRejectNullStudent()
    {
        var course = new Course("Math", "Mathematics", 5, 100m, 500m);

        Assert.Throws<ArgumentNullException>(() =>
            new ExamAttempt(
                null!,
                course,
                7,
                new DateTime(2026, 6, 10)));
    }

    /// <summary>
    /// Verifies that an existing exam attempt stores its student.
    /// </summary>
    [Fact]
    public void ExamAttempt_WithId_ShouldStoreStudent()
    {
        var student = CreateStudent();
        var course = new Course("Math", "Mathematics", 5, 100m, 500m);

        var attempt = new ExamAttempt(
            1,
            student,
            course,
            7,
            new DateTime(2026, 6, 10));

        Assert.Same(student, attempt.Student);
    }

    private static Student CreateStudent()
    {
        return new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "S001",
            new[] { "0722123456" },
            Array.Empty<string>());
    }
}
