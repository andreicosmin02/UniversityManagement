// <copyright file="CourseSelectionService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides operations for selecting courses for students.
/// </summary>
public class CourseSelectionService
{
    private readonly ILogger<CourseSelectionService> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="CourseSelectionService"/> class.
    /// </summary>
    /// <param name="logger">The logger used to record course selection operations.</param>
    public CourseSelectionService(
        ILogger<CourseSelectionService>? logger = null)
    {
        this.logger = logger ?? NullLogger<CourseSelectionService>.Instance;
    }

    /// <summary>
    /// Selects a course for a student in a semester.
    /// </summary>
    /// <param name="student">The student selecting the course.</param>
    /// <param name="course">The course to select.</param>
    /// <param name="semester">The semester associated with the selection.</param>
    /// <param name="existingEnrollments">The existing course selections.</param>
    /// <returns>The newly created enrollment.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the course cannot be selected.
    /// </exception>
    public Enrollment SelectCourse(
        Student student,
        Course course,
        Semester semester,
        IEnumerable<Enrollment> existingEnrollments)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(course);
        ArgumentNullException.ThrowIfNull(semester);
        ArgumentNullException.ThrowIfNull(existingEnrollments);

        if (!semester.Courses.Contains(course))
        {
            this.logger.LogWarning(
                "Course {CourseName} is not available in semester {SemesterNumber}.",
                course.Name,
                semester.Number);

            throw new InvalidOperationException(
                "The course is not available in the selected semester.");
        }

        foreach (var enrollment in existingEnrollments)
        {
            if (ReferenceEquals(enrollment.Student, student)
                && ReferenceEquals(enrollment.Course, course))
            {
                this.logger.LogWarning(
                    "Student {RegistrationNumber} has already selected course {CourseName}.",
                    student.RegistrationNumber,
                    course.Name);

                throw new InvalidOperationException(
                    "The student has already selected this course.");
            }
        }

        var newEnrollment = new Enrollment(student, course, semester);

        this.logger.LogInformation(
            "Student {RegistrationNumber} selected course {CourseName} in semester {SemesterNumber}.",
            student.RegistrationNumber,
            course.Name,
            semester.Number);

        return newEnrollment;
    }

    /// <summary>
    /// Selects a course for a student after validating its prerequisites.
    /// </summary>
    /// <param name="student">The student selecting the course.</param>
    /// <param name="course">The course to select.</param>
    /// <param name="semester">The semester associated with the selection.</param>
    /// <param name="existingEnrollments">The existing course selections.</param>
    /// <param name="examAttempts">The student's previous exam attempts.</param>
    /// <returns>The newly created enrollment.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a prerequisite has not been satisfied.
    /// </exception>
    public Enrollment SelectCourse(
        Student student,
        Course course,
        Semester semester,
        IEnumerable<Enrollment> existingEnrollments,
        IEnumerable<ExamAttempt> examAttempts)
    {
        ArgumentNullException.ThrowIfNull(examAttempts);

        foreach (var prerequisite in course.Prerequisites)
        {
            var prerequisiteSatisfied = examAttempts.Any(
                attempt =>
                    ReferenceEquals(attempt.Course, prerequisite.RequiredCourse)
                    && attempt.Grade >= prerequisite.MinimumGrade);

            if (!prerequisiteSatisfied)
            {
                throw new InvalidOperationException(
                    "The student has not satisfied all course prerequisites.");
            }
        }

        return this.SelectCourse(
            student,
            course,
            semester,
            existingEnrollments);
    }
}
