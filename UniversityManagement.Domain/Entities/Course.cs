// <copyright file="Course.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

/// <summary>
/// Represents a university course and its credit and pricing information.
/// </summary>
public class Course
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Course"/> class.
    /// </summary>
    /// <param name="name">The course name.</param>
    /// <param name="description">The course description.</param>
    /// <param name="credits">The number of credits awarded by the course.</param>
    /// <param name="minimumCostPerCredit">The minimum cost for each credit.</param>
    /// <param name="cost">The total course cost.</param>
    public Course(
        string name,
        string description,
        int credits,
        decimal minimumCostPerCredit,
        decimal cost)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Course name cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Course description cannot be empty.");
        }

        if (credits <= 0)
        {
            throw new ArgumentException("Course credits must be greater than zero.");
        }

        decimal minimumCost = minimumCostPerCredit * credits;
        decimal maximumCost = 2 * minimumCostPerCredit * credits;

        if (cost < minimumCost || cost > maximumCost)
        {
            throw new ArgumentException("Course cost is outside the allowed range.");
        }

        if (minimumCostPerCredit <= 0)
        {
            throw new ArgumentException("Minimum cost per credit must be greater than zero.");
        }

        this.Name = name;
        this.Description = description;
        this.Credits = credits;
        this.MinimumCostPerCredit = minimumCostPerCredit;
        this.Cost = cost;
    }

    /// <summary>
    /// Gets the course name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the number of credits awarded by the course.
    /// </summary>
    public int Credits { get; }

    /// <summary>
    /// Gets the course description.
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Gets the minimum cost for each credit.
    /// </summary>
    public decimal MinimumCostPerCredit { get; }

    /// <summary>
    /// Gets the total course cost.
    /// </summary>
    public decimal Cost { get; }
}
