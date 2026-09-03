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
        var student = CreateStudent("S001");
        var courseA = new Course("A", "Course A", 6, 100m, 600m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var semester = new Semester(1, 0);
        semester.AddCourse(courseA);
        semester.AddCourse(courseB);

        var attempts = new[]
        {
            new ExamAttempt(
                student,
                courseA,
                7,
                new DateTime(2026, 6, 1)),
            new ExamAttempt(
                student,
                courseB,
                4,
                new DateTime(2026, 6, 2)),
        };

        var service = new AcademicReportingService();

        var credits = service.GetEarnedCredits(student, semester, attempts);

        Assert.Equal(6, credits);
    }

    /// <summary>
    /// Verifies that another student's attempts do not contribute earned credits.
    /// </summary>
    [Fact]
    public void GetEarnedCredits_ShouldIgnoreOtherStudentsAttempts()
    {
        var student = CreateStudent("S001");
        var otherStudent = CreateStudent("S002");
        var course = new Course("A", "Course A", 6, 100m, 600m);
        var semester = new Semester(1, 0);
        semester.AddCourse(course);
        var attempts = new[]
        {
            new ExamAttempt(otherStudent, course, 10, new DateTime(2026, 6, 1)),
        };
        var service = new AcademicReportingService();

        var credits = service.GetEarnedCredits(student, semester, attempts);

        Assert.Equal(0, credits);
    }

    /// <summary>
    /// Verifies that earned-credit reporting rejects a null student.
    /// </summary>
    [Fact]
    public void GetEarnedCredits_ShouldRejectNullStudent()
    {
        var semester = new Semester(1, 0);
        var service = new AcademicReportingService();

        Assert.Throws<ArgumentNullException>(
            () => service.GetEarnedCredits(
                null!,
                semester,
                Array.Empty<ExamAttempt>()));
    }

    /// <summary>
    /// Verifies that earned-credit reporting rejects a null semester.
    /// </summary>
    [Fact]
    public void GetEarnedCredits_ShouldRejectNullSemester()
    {
        var student = CreateStudent("S001");
        var service = new AcademicReportingService();

        Assert.Throws<ArgumentNullException>(
            () => service.GetEarnedCredits(
                student,
                null!,
                Array.Empty<ExamAttempt>()));
    }

    /// <summary>
    /// Verifies that earned-credit reporting rejects a null exam history.
    /// </summary>
    [Fact]
    public void GetEarnedCredits_ShouldRejectNullExamAttempts()
    {
        var student = CreateStudent("S001");
        var semester = new Semester(1, 0);
        var service = new AcademicReportingService();

        Assert.Throws<ArgumentNullException>(
            () => service.GetEarnedCredits(student, semester, null!));
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
        var student = CreateStudent("S001");
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var attempts = new[]
        {
            new ExamAttempt(
                student,
                courseA,
                4,
                new DateTime(2026, 6, 1)),
            new ExamAttempt(
                student,
                courseA,
                5,
                new DateTime(2026, 6, 2)),
            new ExamAttempt(
                student,
                courseA,
                7,
                new DateTime(2026, 6, 3)),
            new ExamAttempt(
                student,
                courseB,
                10,
                new DateTime(2026, 6, 4)),
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

    /// <summary>
    /// Verifies that students who passed all selected courses are counted as integral students.
    /// </summary>
    [Fact]
    public void GetIntegralStudentCount_ShouldCountStudentsWhoPassedAllSelectedCourses()
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

        var attemptsByStudent =
            new Dictionary<Student, IEnumerable<ExamAttempt>>
            {
                [studentA] = new[]
                {
                    new ExamAttempt(
                        studentA,
                        courseA,
                        7,
                        new DateTime(2026, 6, 1)),
                    new ExamAttempt(
                        studentA,
                        courseB,
                        5,
                        new DateTime(2026, 6, 2)),
                },
                [studentB] = new[]
                {
                    new ExamAttempt(
                        studentB,
                        courseA,
                        4,
                        new DateTime(2026, 6, 1)),
                },
            };

        var service = new AcademicReportingService();

        var count = service.GetIntegralStudentCount(
            new[] { studentA, studentB },
            enrollments,
            attemptsByStudent);

        Assert.Equal(1, count);
    }

    /// <summary>
    /// Verifies that a missing exam for a selected course prevents integral status.
    /// </summary>
    [Fact]
    public void GetIntegralStudentCount_ShouldRejectStudentWithMissingPassedCourse()
    {
        var student = CreateStudent("S001");

        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);
        var semester = new Semester(1, 0);

        var enrollments = new[]
        {
            new Enrollment(student, courseA, semester),
            new Enrollment(student, courseB, semester),
        };

        var attemptsByStudent =
            new Dictionary<Student, IEnumerable<ExamAttempt>>
            {
                [student] = new[]
                {
                    new ExamAttempt(
                        student,
                        courseA,
                        8,
                        new DateTime(2026, 6, 1)),
                },
            };

        var service = new AcademicReportingService();

        var count = service.GetIntegralStudentCount(
            new[] { student },
            enrollments,
            attemptsByStudent);

        Assert.Equal(0, count);
    }

    /// <summary>
    /// Verifies that dictionary entries cannot contain another student's attempts.
    /// </summary>
    [Fact]
    public void GetIntegralStudentCount_ShouldIgnoreAttemptsBelongingToDifferentStudent()
    {
        var student = CreateStudent("S001");
        var otherStudent = CreateStudent("S002");
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        var enrollments = new[]
        {
            new Enrollment(student, course, semester),
        };
        var attemptsByStudent =
            new Dictionary<Student, IEnumerable<ExamAttempt>>
            {
                [student] = new[]
                {
                    new ExamAttempt(
                        otherStudent,
                        course,
                        10,
                        new DateTime(2026, 6, 1)),
                },
            };
        var service = new AcademicReportingService();

        var count = service.GetIntegralStudentCount(
            new[] { student },
            enrollments,
            attemptsByStudent);

        Assert.Equal(0, count);
    }

    /// <summary>
    /// Verifies that a course with no passing grades has an average of zero.
    /// </summary>
    [Fact]
    public void GetAveragePassingGrade_ShouldReturnZeroWhenThereAreNoPassingGrades()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var service = new AcademicReportingService();

        var average = service.GetAveragePassingGrade(
            course,
            Array.Empty<ExamAttempt>());

        Assert.Equal(0m, average);
    }

    /// <summary>
    /// Verifies that the average number of courses is zero when there are no enrollments.
    /// </summary>
    [Fact]
    public void GetAverageCoursesPerStudent_ShouldReturnZeroWhenThereAreNoEnrollments()
    {
        var service = new AcademicReportingService();

        var average = service.GetAverageCoursesPerStudent(
            Array.Empty<Enrollment>());

        Assert.Equal(0m, average);
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
