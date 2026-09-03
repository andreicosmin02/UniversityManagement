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
        var student = CreateStudent();
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var semester = new Semester(1, 10);
        semester.AddCourse(courseA);
        semester.AddCourse(courseB);

        var attempts = new[]
        {
            new ExamAttempt(
                student,
                courseA,
                5,
                new DateTime(2026, 6, 1)),
            new ExamAttempt(
                student,
                courseB,
                7,
                new DateTime(2026, 6, 2)),
        };

        var service = new PromotionService();

        var result = service.CanPromote(student, semester, attempts);

        Assert.True(result);
    }

    /// <summary>
    /// Verifies that promotion is rejected when the minimum credit threshold is not reached.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldReturnFalseWhenMinimumCreditsAreNotReached()
    {
        var student = CreateStudent();
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var semester = new Semester(1, 10);
        semester.AddCourse(courseA);
        semester.AddCourse(courseB);

        var attempts = new[]
        {
            new ExamAttempt(
                student,
                courseA,
                5,
                new DateTime(2026, 6, 1)),
            new ExamAttempt(
                student,
                courseB,
                4,
                new DateTime(2026, 6, 2)),
        };

        var service = new PromotionService();

        var result = service.CanPromote(student, semester, attempts);

        Assert.False(result);
    }

    /// <summary>
    /// Verifies that failed courses do not contribute credits toward promotion.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldCountOnlyPassedCourses()
    {
        var student = CreateStudent();
        var passedCourse = new Course("A", "Course A", 6, 100m, 600m);
        var failedCourse = new Course("B", "Course B", 10, 100m, 1000m);

        var semester = new Semester(1, 10);
        semester.AddCourse(passedCourse);
        semester.AddCourse(failedCourse);

        var attempts = new[]
        {
            new ExamAttempt(
                student,
                passedCourse,
                5,
                new DateTime(2026, 6, 1)),
            new ExamAttempt(
                student,
                failedCourse,
                4,
                new DateTime(2026, 6, 2)),
        };

        var service = new PromotionService();

        Assert.False(service.CanPromote(student, semester, attempts));
    }

    /// <summary>
    /// Verifies that another student's passed courses do not grant promotion.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldIgnoreOtherStudentsPassedCourses()
    {
        var student = CreateStudent("S001");
        var otherStudent = CreateStudent("S002");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 5);
        semester.AddCourse(course);
        var attempts = new[]
        {
            new ExamAttempt(otherStudent, course, 10, new DateTime(2026, 6, 1)),
        };
        var service = new PromotionService();

        Assert.False(service.CanPromote(student, semester, attempts));
    }

    /// <summary>
    /// Verifies that promotion validation rejects a null student.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldRejectNullStudent()
    {
        var semester = new Semester(1, 0);
        var service = new PromotionService();

        Assert.Throws<ArgumentNullException>(
            () => service.CanPromote(
                null!,
                semester,
                Array.Empty<ExamAttempt>()));
    }

    /// <summary>
    /// Verifies that promotion validation rejects a null semester.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldRejectNullSemester()
    {
        var student = CreateStudent();
        var service = new PromotionService();

        Assert.Throws<ArgumentNullException>(
            () => service.CanPromote(
                student,
                null!,
                Array.Empty<ExamAttempt>()));
    }

    /// <summary>
    /// Verifies that promotion validation rejects a null exam history.
    /// </summary>
    [Fact]
    public void CanPromote_ShouldRejectNullExamAttempts()
    {
        var student = CreateStudent();
        var semester = new Semester(1, 0);
        var service = new PromotionService();

        Assert.Throws<ArgumentNullException>(
            () => service.CanPromote(student, semester, null!));
    }

    private static Student CreateStudent(string registrationNumber = "S001")
    {
        return new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            registrationNumber,
            new[] { "0722123456" },
            Array.Empty<string>());
    }
}
