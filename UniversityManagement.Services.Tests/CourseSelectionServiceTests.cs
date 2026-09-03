// <copyright file="CourseSelectionServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using Microsoft.Extensions.Logging;
using Moq;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests course selection operations.
/// </summary>
public class CourseSelectionServiceTests
{
    /// <summary>
    /// Verifies that a course outside the selected semester cannot be chosen.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectCourseNotAvailableInSemester()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        var service = new CourseSelectionService();

        Assert.Throws<InvalidOperationException>(
            () => service.SelectCourse(
                student,
                course,
                semester,
                Array.Empty<Enrollment>()));
    }

    /// <summary>
    /// Verifies that a student cannot choose the same course twice.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectDuplicateCourseForSameStudent()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        semester.AddCourse(course);

        var existingEnrollment = new Enrollment(student, course, semester);
        var service = new CourseSelectionService();

        Assert.Throws<InvalidOperationException>(
            () => service.SelectCourse(
                student,
                course,
                semester,
                new[] { existingEnrollment }));
    }

    /// <summary>
    /// Verifies that a valid course selection creates an enrollment.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldCreateEnrollmentForValidSelection()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        semester.AddCourse(course);

        var service = new CourseSelectionService();

        var enrollment = service.SelectCourse(
            student,
            course,
            semester,
            Array.Empty<Enrollment>());

        Assert.Same(student, enrollment.Student);
        Assert.Same(course, enrollment.Course);
        Assert.Same(semester, enrollment.Semester);
    }

    /// <summary>
    /// Verifies that a null student is rejected.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectNullStudent()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        semester.AddCourse(course);

        var service = new CourseSelectionService();

        Assert.Throws<ArgumentNullException>(
            () => service.SelectCourse(
                null!,
                course,
                semester,
                Array.Empty<Enrollment>()));
    }

    /// <summary>
    /// Verifies that a null course is rejected.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectNullCourse()
    {
        var student = CreateStudent("S001");
        var semester = new Semester(1, 0);
        var service = new CourseSelectionService();

        Assert.Throws<ArgumentNullException>(
            () => service.SelectCourse(
                student,
                null!,
                semester,
                Array.Empty<Enrollment>()));
    }

    /// <summary>
    /// Verifies that a null semester is rejected.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectNullSemester()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var service = new CourseSelectionService();

        Assert.Throws<ArgumentNullException>(
            () => service.SelectCourse(
                student,
                course,
                null!,
                Array.Empty<Enrollment>()));
    }

    /// <summary>
    /// Verifies that a null enrollment collection is rejected.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectNullExistingEnrollments()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        semester.AddCourse(course);

        var service = new CourseSelectionService();

        Assert.Throws<ArgumentNullException>(
            () => service.SelectCourse(
                student,
                course,
                semester,
                null!));
    }

    /// <summary>
    /// Verifies that a course cannot be selected when a prerequisite was not passed.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectMissingPrerequisite()
    {
        var student = CreateStudent("S001");
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        course.AddPrerequisite(new Prerequisite(requiredCourse, 7));

        var semester = new Semester(2, 0);
        semester.AddCourse(course);

        var service = new CourseSelectionService();

        Assert.Throws<InvalidOperationException>(
            () => service.SelectCourse(
                student,
                course,
                semester,
                Array.Empty<Enrollment>(),
                Array.Empty<ExamAttempt>()));
    }

    /// <summary>
    /// Verifies that a course can be selected when its prerequisite was passed
    /// with the required grade.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldAcceptSatisfiedPrerequisite()
    {
        var student = CreateStudent("S001");
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        course.AddPrerequisite(new Prerequisite(requiredCourse, 7));

        var semester = new Semester(2, 0);
        semester.AddCourse(course);

        var examAttempts = new[]
        {
        new ExamAttempt(requiredCourse, 7, new DateTime(2026, 6, 10)),
        };

        var service = new CourseSelectionService();

        var enrollment = service.SelectCourse(
            student,
            course,
            semester,
            Array.Empty<Enrollment>(),
            examAttempts);

        Assert.Same(course, enrollment.Course);
    }

    /// <summary>
    /// Verifies that passing a prerequisite below its required grade is insufficient.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectPrerequisiteBelowRequiredGrade()
    {
        var student = CreateStudent("S001");
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        course.AddPrerequisite(new Prerequisite(requiredCourse, 7));

        var semester = new Semester(2, 0);
        semester.AddCourse(course);

        var examAttempts = new[]
        {
        new ExamAttempt(requiredCourse, 6, new DateTime(2026, 6, 10)),
        };

        var service = new CourseSelectionService();

        Assert.Throws<InvalidOperationException>(
            () => service.SelectCourse(
                student,
                course,
                semester,
                Array.Empty<Enrollment>(),
                examAttempts));
    }

    /// <summary>
    /// Verifies that a null exam attempt collection is rejected.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldRejectNullExamAttempts()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        semester.AddCourse(course);

        var service = new CourseSelectionService();

        Assert.Throws<ArgumentNullException>(
            () => service.SelectCourse(
                student,
                course,
                semester,
                Array.Empty<Enrollment>(),
                null!));
    }

    /// <summary>
    /// Verifies that a successful course selection is logged.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldLogSuccessfulSelection()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        semester.AddCourse(course);

        var logger = new Mock<ILogger<CourseSelectionService>>();
        var service = new CourseSelectionService(logger.Object);

        service.SelectCourse(
            student,
            course,
            semester,
            Array.Empty<Enrollment>());

        logger.Verify(
            loggerInstance => loggerInstance.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>(
                    (value, type) =>
                        value.ToString() !.Contains("selected")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that an unavailable course selection is logged as a warning.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldLogWarningWhenCourseIsUnavailable()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);

        var logger = new Mock<ILogger<CourseSelectionService>>();
        var service = new CourseSelectionService(logger.Object);

        Assert.Throws<InvalidOperationException>(() =>
            service.SelectCourse(
                student,
                course,
                semester,
                Array.Empty<Enrollment>()));

        logger.Verify(
            loggerInstance => loggerInstance.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>(
                    (value, type) =>
                        value.ToString() !.Contains("not available")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that a duplicate course selection is logged as a warning.
    /// </summary>
    [Fact]
    public void SelectCourse_ShouldLogWarningWhenCourseIsAlreadySelected()
    {
        var student = CreateStudent("S001");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        semester.AddCourse(course);

        var existingEnrollment = new Enrollment(student, course, semester);

        var logger = new Mock<ILogger<CourseSelectionService>>();
        var service = new CourseSelectionService(logger.Object);

        Assert.Throws<InvalidOperationException>(() =>
            service.SelectCourse(
                student,
                course,
                semester,
                new[] { existingEnrollment }));

        logger.Verify(
            loggerInstance => loggerInstance.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>(
                    (value, type) =>
                        value.ToString() !.Contains("already selected")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
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
