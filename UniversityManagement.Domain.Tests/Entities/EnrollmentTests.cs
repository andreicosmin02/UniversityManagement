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
}
