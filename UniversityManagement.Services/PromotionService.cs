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
    /// Determines whether the student has earned enough credits to pass the semester.
    /// </summary>
    /// <param name="semester">The semester to evaluate.</param>
    /// <param name="examAttempts">The student's exam attempts.</param>
    /// <returns>
    /// <see langword="true"/> when the minimum number of credits has been earned;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    public bool CanPromote(
        Semester semester,
        IEnumerable<ExamAttempt> examAttempts)
    {
        ArgumentNullException.ThrowIfNull(semester);
        ArgumentNullException.ThrowIfNull(examAttempts);

        var earnedCredits = semester.Courses
            .Where(course => examAttempts.Any(
                attempt =>
                    ReferenceEquals(attempt.Course, course)
                    && attempt.Passed))
            .Sum(course => course.Credits);

        return earnedCredits >= semester.MinimumCredits;
    }
}
