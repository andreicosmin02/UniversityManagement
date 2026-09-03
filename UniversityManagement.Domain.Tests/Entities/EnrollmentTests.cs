// <copyright file="EnrollmentTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Tests.Entities;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests the <see cref="Enrollment"/> entity.
/// </summary>
public class EnrollmentTests
{
    /// <summary>
    /// Verifies that an enrollment stores its student, course, and semester.
    /// </summary>
    [Fact]
    public void Enrollment_ShouldStoreStudentCourseAndSemester()
    {
        var student = new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "S001",
            new[] { "0722123456" },
            Array.Empty<string>());

        var course = new Course("Programming", "Programming course", 5, 100m, 500m);
        var semester = new Semester(1, 0);

        var enrollment = new Enrollment(student, course, semester);

        Assert.Same(student, enrollment.Student);
        Assert.Same(course, enrollment.Course);
        Assert.Same(semester, enrollment.Semester);
    }

    /// <summary>
    /// Verifies that a new enrollment starts without a persistent identifier.
    /// </summary>
    [Fact]
    public void Enrollment_ShouldStartWithZeroId()
    {
        var student = new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "12345",
            new[] { "0722123456" },
            Array.Empty<string>());
        var course = new Course("Math", "Mathematics", 5, 100, 500);
        var semester = new Semester(1, 30);

        var enrollment = new Enrollment(student, course, semester);

        Assert.Equal(0, enrollment.Id);
    }

    /// <summary>
    /// Verifies that an existing enrollment can store a persistent identifier.
    /// </summary>
    [Fact]
    public void Enrollment_ShouldStorePositiveId()
    {
        var student = new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "12345",
            new[] { "0722123456" },
            Array.Empty<string>());
        var course = new Course("Math", "Mathematics", 5, 100, 500);
        var semester = new Semester(1, 30);

        var enrollment = new Enrollment(1, student, course, semester);

        Assert.Equal(1, enrollment.Id);
    }

    /// <summary>
    /// Verifies that an existing enrollment rejects a non-positive identifier.
    /// </summary>
    /// <param name="id">The invalid identifier to test.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Enrollment_ShouldRejectNonPositiveId(int id)
    {
        var student = new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "12345",
            new[] { "0722123456" },
            Array.Empty<string>());
        var course = new Course("Math", "Mathematics", 5, 100, 500);
        var semester = new Semester(1, 30);

        Assert.Throws<ArgumentException>(
            () => new Enrollment(id, student, course, semester));
    }
}
