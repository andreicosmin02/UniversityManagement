// <copyright file="AcademicReportingService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using System.Collections.Generic;
using System.Linq;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides academic reporting operations.
/// </summary>
public class AcademicReportingService
{
    /// <summary>
    /// Gets the number of credits earned in a semester.
    /// </summary>
    /// <param name="semester">The semester to evaluate.</param>
    /// <param name="examAttempts">The student's exam attempts.</param>
    /// <returns>The number of earned credits.</returns>
    public int GetEarnedCredits(
        Semester semester,
        IEnumerable<ExamAttempt> examAttempts)
    {
        ArgumentNullException.ThrowIfNull(semester);
        ArgumentNullException.ThrowIfNull(examAttempts);

        return semester.Courses
            .Where(course => examAttempts.Any(
                attempt =>
                    ReferenceEquals(attempt.Course, course)
                    && attempt.Passed))
            .Sum(course => course.Credits);
    }

    /// <summary>
    /// Gets the number of students enrolled in a course.
    /// </summary>
    /// <param name="course">The course to evaluate.</param>
    /// <param name="enrollments">The existing enrollments.</param>
    /// <returns>The number of enrolled students.</returns>
    public int GetEnrolledStudentCount(
        Course course,
        IEnumerable<Enrollment> enrollments)
    {
        ArgumentNullException.ThrowIfNull(course);
        ArgumentNullException.ThrowIfNull(enrollments);

        return enrollments
            .Where(enrollment => ReferenceEquals(enrollment.Course, course))
            .Select(enrollment => enrollment.Student)
            .Distinct()
            .Count();
    }

    /// <summary>
    /// Gets the average passing grade for a course.
    /// </summary>
    /// <param name="course">The course to evaluate.</param>
    /// <param name="examAttempts">The exam attempts to evaluate.</param>
    /// <returns>The average of passing grades.</returns>
    public decimal GetAveragePassingGrade(
        Course course,
        IEnumerable<ExamAttempt> examAttempts)
    {
        ArgumentNullException.ThrowIfNull(course);
        ArgumentNullException.ThrowIfNull(examAttempts);

        return examAttempts
            .Where(attempt =>
                ReferenceEquals(attempt.Course, course)
                && attempt.Passed)
            .Average(attempt => (decimal)attempt.Grade);
    }

    /// <summary>
    /// Gets the average number of selected courses per student.
    /// </summary>
    /// <param name="enrollments">The existing enrollments.</param>
    /// <returns>The average number of courses selected per student.</returns>
    public decimal GetAverageCoursesPerStudent(
        IEnumerable<Enrollment> enrollments)
    {
        ArgumentNullException.ThrowIfNull(enrollments);

        return enrollments
            .GroupBy(enrollment => enrollment.Student)
            .Average(group => (decimal)group.Count());
    }
}
