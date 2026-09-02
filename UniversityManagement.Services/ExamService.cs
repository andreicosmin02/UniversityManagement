// <copyright file="ExamService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using System.Collections.Generic;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides operations for registering exam attempts.
/// </summary>
public class ExamService
{
    private readonly int maximumAttempts;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExamService"/> class.
    /// </summary>
    /// <param name="maximumAttempts">The maximum number of exam attempts allowed per course.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the maximum number of attempts is not positive.
    /// </exception>
    public ExamService(int maximumAttempts)
    {
        if (maximumAttempts <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumAttempts),
                "The maximum number of attempts must be greater than zero.");
        }

        this.maximumAttempts = maximumAttempts;
    }

    /// <summary>
    /// Registers a new exam attempt when the course has not already been passed.
    /// </summary>
    /// <param name="course">The course for which the exam is taken.</param>
    /// <param name="grade">The obtained grade.</param>
    /// <param name="examDate">The examination date.</param>
    /// <param name="previousAttempts">The student's previous exam attempts.</param>
    /// <returns>The newly registered exam attempt.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the course has already been passed.
    /// </exception>
    public ExamAttempt RegisterAttempt(
    Course course,
    int grade,
    DateTime examDate,
    IEnumerable<ExamAttempt> previousAttempts)
    {
        ArgumentNullException.ThrowIfNull(previousAttempts);

        foreach (var attempt in previousAttempts)
        {
            if (attempt.ExamDate.Date == examDate.Date)
            {
                throw new InvalidOperationException(
                    "The student cannot take more than one exam on the same day.");
            }

            if (ReferenceEquals(attempt.Course, course) && attempt.Passed)
            {
                throw new InvalidOperationException(
                    "The student cannot take the exam again after passing the course.");
            }
        }

        var courseAttemptCount = previousAttempts.Count(
            attempt => ReferenceEquals(attempt.Course, course));

        if (courseAttemptCount >= this.maximumAttempts)
        {
            throw new InvalidOperationException(
                "The maximum number of exam attempts has been reached.");
        }

        foreach (var prerequisite in course.Prerequisites)
        {
            var prerequisiteSatisfied = previousAttempts.Any(
                attempt =>
                    ReferenceEquals(attempt.Course, prerequisite.RequiredCourse)
                    && attempt.Grade >= prerequisite.MinimumGrade);

            if (!prerequisiteSatisfied)
            {
                throw new InvalidOperationException(
                    "The student has not satisfied all course prerequisites.");
            }
        }

        return new ExamAttempt(course, grade, examDate);
    }
}
