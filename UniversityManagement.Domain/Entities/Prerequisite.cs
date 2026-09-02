// <copyright file="Prerequisite.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

/// <summary>
/// Represents a course prerequisite and the minimum required grade.
/// </summary>
public class Prerequisite
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Prerequisite"/> class.
    /// </summary>
    /// <param name="requiredCourse">The course that must be completed.</param>
    /// <param name="minimumGrade">The minimum required grade.</param>
    public Prerequisite(Course requiredCourse, int minimumGrade)
    {
        ArgumentNullException.ThrowIfNull(requiredCourse);

        if (minimumGrade < 1 || minimumGrade > 10)
        {
            throw new ArgumentException(
                "Minimum grade must be between 1 and 10.",
                nameof(minimumGrade));
        }

        this.RequiredCourse = requiredCourse;
        this.MinimumGrade = minimumGrade;
    }

    /// <summary>
    /// Gets the course that must be completed.
    /// </summary>
    public Course RequiredCourse { get; }

    /// <summary>
    /// Gets the minimum required grade.
    /// </summary>
    public int MinimumGrade { get; }
}
