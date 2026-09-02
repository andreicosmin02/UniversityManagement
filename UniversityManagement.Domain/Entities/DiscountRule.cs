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
        if (percentage <= 0m || percentage > 100m)
        {
            throw new ArgumentOutOfRangeException(
                nameof(percentage),
                "The discount percentage must be between 1 and 100.");
        }

        this.courses = new List<Course>(courses);
        this.Percentage = percentage;
    }

    /// <summary>
    /// Gets the courses required by the discount rule.
    /// </summary>
    public IReadOnlyCollection<Course> Courses => this.courses;

    /// <summary>
    /// Gets the discount percentage.
    /// </summary>
    public decimal Percentage { get; }
}
