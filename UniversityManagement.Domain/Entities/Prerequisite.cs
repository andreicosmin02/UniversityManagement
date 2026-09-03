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
    /// Initializes a new instance of the <see cref="Prerequisite"/> class
    /// with an existing persistent identifier.
    /// </summary>
    /// <param name="id">The persistent identifier.</param>
    /// <param name="requiredCourse">The required course.</param>
    /// <param name="minimumGrade">The minimum required grade.</param>
    public Prerequisite(
        int id,
        Course requiredCourse,
        int minimumGrade)
        : this(requiredCourse, minimumGrade)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Prerequisite identifier must be greater than zero.",
                nameof(id));
        }

        this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Prerequisite"/> class for persistence.
    /// </summary>
    private Prerequisite()
    {
        this.RequiredCourse = null!;
    }

    /// <summary>
    /// Gets the persistent identifier of the prerequisite.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the required course.
    /// </summary>
    public Course RequiredCourse { get; private set; }

    /// <summary>
    /// Gets the minimum required grade.
    /// </summary>
    public int MinimumGrade { get; private set; }
}
