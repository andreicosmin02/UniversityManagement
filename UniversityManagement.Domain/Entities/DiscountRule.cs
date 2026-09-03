// <copyright file="DiscountRule.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

using System.Collections.Generic;

/// <summary>
/// Represents a discount applied when a predefined combination of courses is selected.
/// </summary>
public class DiscountRule
{
    private readonly List<Course> courses;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscountRule"/> class.
    /// </summary>
    /// <param name="courses">The courses required for the discount.</param>
    /// <param name="percentage">The discount percentage.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the discount percentage is not between 1 and 100.
    /// </exception>
    public DiscountRule(IEnumerable<Course> courses, decimal percentage)
    {
        ArgumentNullException.ThrowIfNull(courses);

        var courseList = courses.ToList();

        if (courseList.Count == 0)
        {
            throw new ArgumentException(
                "The discount combination must contain at least one course.",
                nameof(courses));
        }

        if (courseList.Any(course => course is null))
        {
            throw new ArgumentException(
                "The discount combination cannot contain a null course.",
                nameof(courses));
        }

        if (courseList.Distinct().Count() != courseList.Count)
        {
            throw new ArgumentException(
                "The discount combination cannot contain duplicate courses.",
                nameof(courses));
        }

        if (percentage <= 0m || percentage > 100m)
        {
            throw new ArgumentOutOfRangeException(
                nameof(percentage),
                "The discount percentage must be between 1 and 100.");
        }

        this.courses = courseList;
        this.Percentage = percentage;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscountRule"/> class
    /// with an existing persistent identifier.
    /// </summary>
    /// <param name="id">The persistent identifier.</param>
    /// <param name="courses">The courses included in the discount combination.</param>
    /// <param name="percentage">The discount percentage.</param>
    public DiscountRule(
        int id,
        IEnumerable<Course> courses,
        decimal percentage)
        : this(courses, percentage)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Discount rule identifier must be greater than zero.",
                nameof(id));
        }

        this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscountRule"/> class for persistence.
    /// </summary>
    private DiscountRule()
    {
        this.courses = new List<Course>();
    }

    /// <summary>
    /// Gets the persistent identifier of the discount rule.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the courses required by the discount rule.
    /// </summary>
    public IReadOnlyCollection<Course> Courses => this.courses;

    /// <summary>
    /// Gets the discount percentage.
    /// </summary>
    public decimal Percentage { get; private set; }
}
