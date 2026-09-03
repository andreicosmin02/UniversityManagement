// <copyright file="PromotionService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using System.Collections.Generic;
using System.Linq;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides operations for determining semester promotion eligibility.
/// </summary>
public class PromotionService
{
    /// <summary>
    /// Determines whether a student has earned enough credits to pass the semester.
    /// </summary>
    /// <param name="student">The student to evaluate.</param>
    /// <param name="semester">The semester to evaluate.</param>
    /// <param name="examAttempts">The exam attempts to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> when the minimum number of credits has been earned;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanPromote(
        Student student,
        Semester semester,
        IEnumerable<ExamAttempt> examAttempts)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(semester);
        ArgumentNullException.ThrowIfNull(examAttempts);

        var earnedCredits = semester.Courses
            .Where(course => examAttempts.Any(
                attempt =>
                    ReferenceEquals(attempt.Student, student)
                    && ReferenceEquals(attempt.Course, course)
                    && attempt.Passed))
            .Sum(course => course.Credits);

        return earnedCredits >= semester.MinimumCredits;
    }
}
