// <copyright file="Semester.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

/// <summary>
/// Represents a semester and the courses available during it.
/// </summary>
public class Semester
{
    private readonly List<Course> courses = new List<Course>();

    /// <summary>
    /// Initializes a new instance of the <see cref="Semester"/> class.
    /// </summary>
    /// <param name="number">The semester number.</param>
    /// <param name="minimumCredits">The minimum number of available credits required.</param>
    public Semester(int number, int minimumCredits)
    {
        if (number <= 0)
        {
            throw new ArgumentException("Semester number must be greater than zero.");
        }

        if (minimumCredits < 0)
        {
            throw new ArgumentException("Minimum credits cannot be negative.");
        }

        this.Number = number;
        this.MinimumCredits = minimumCredits;
    }

    /// <summary>
    /// Gets the semester number.
    /// </summary>
    public int Number { get; }

    /// <summary>
    /// Gets the minimum number of available credits required by the semester.
    /// </summary>
    public int MinimumCredits { get; }

    /// <summary>
    /// Gets the courses available during the semester.
    /// </summary>
    public IReadOnlyCollection<Course> Courses => this.courses;

    /// <summary>
    /// Gets the total number of credits available from the semester's courses.
    /// </summary>
    public int TotalAvailableCredits =>
        this.courses.Sum(course => course.Credits);

    /// <summary>
    /// Gets a value indicating whether enough credits are available.
    /// </summary>
    public bool HasEnoughAvailableCredits =>
        this.TotalAvailableCredits >= this.MinimumCredits;

    /// <summary>
    /// Adds a course to the semester.
    /// </summary>
    /// <param name="course">The course to add.</param>
    public void AddCourse(Course course)
    {
        ArgumentNullException.ThrowIfNull(course);

        if (this.courses.Contains(course))
        {
            throw new ArgumentException("Course is already added to this semester.");
        }

        this.courses.Add(course);
    }
}
