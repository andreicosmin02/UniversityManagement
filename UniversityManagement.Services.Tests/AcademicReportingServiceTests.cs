// <copyright file="AcademicReportingServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests academic reporting operations.
/// </summary>
public class AcademicReportingServiceTests
{
    /// <summary>
    /// Verifies that only passed courses contribute earned credits.
    /// </summary>
    [Fact]
    public void GetEarnedCredits_ShouldCountOnlyPassedCourses()
    {
        var courseA = new Course("A", "Course A", 6, 100m, 600m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var semester = new Semester(1, 0);
        semester.AddCourse(courseA);
        semester.AddCourse(courseB);

        var attempts = new[]
        {
            new ExamAttempt(courseA, 7, new DateTime(2026, 6, 1)),
            new ExamAttempt(courseB, 4, new DateTime(2026, 6, 2)),
        };

        var service = new AcademicReportingService();

        var credits = service.GetEarnedCredits(semester, attempts);

        Assert.Equal(6, credits);
    }

    /// <summary>
    /// Verifies that enrolled students are counted for the specified course.
    /// </summary>
    [Fact]
    public void GetEnrolledStudentCount_ShouldCountStudentsForCourse()
    {
        var studentA = CreateStudent("S001");
        var studentB = CreateStudent("S002");

        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);
        var semester = new Semester(1, 0);

        var enrollments = new[]
        {
            new Enrollment(studentA, courseA, semester),
            new Enrollment(studentB, courseA, semester),
            new Enrollment(studentA, courseB, semester),
        };

        var service = new AcademicReportingService();

        var count = service.GetEnrolledStudentCount(courseA, enrollments);

        Assert.Equal(2, count);
    }

    /// <summary>
    /// Verifies that only passing grades are included in the course average.
    /// </summary>
    [Fact]
    public void GetAveragePassingGrade_ShouldUseOnlyPassingGrades()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var attempts = new[]
        {
            new ExamAttempt(courseA, 4, new DateTime(2026, 6, 1)),
            new ExamAttempt(courseA, 5, new DateTime(2026, 6, 2)),
            new ExamAttempt(courseA, 7, new DateTime(2026, 6, 3)),
            new ExamAttempt(courseB, 10, new DateTime(2026, 6, 4)),
        };

        var service = new AcademicReportingService();

        var average = service.GetAveragePassingGrade(courseA, attempts);

        Assert.Equal(6m, average);
    }

    /// <summary>
    /// Verifies that the average number of selected courses per student is calculated.
    /// </summary>
    [Fact]
    public void GetAverageCoursesPerStudent_ShouldCalculateAverage()
    {
        var studentA = CreateStudent("S001");
        var studentB = CreateStudent("S002");

        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);
        var semester = new Semester(1, 0);

        var enrollments = new[]
        {
            new Enrollment(studentA, courseA, semester),
            new Enrollment(studentA, courseB, semester),
            new Enrollment(studentB, courseA, semester),
        };

        var service = new AcademicReportingService();

        var average = service.GetAverageCoursesPerStudent(enrollments);

        Assert.Equal(1.5m, average);
    }

    private static Student CreateStudent(string registrationNumber)
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
