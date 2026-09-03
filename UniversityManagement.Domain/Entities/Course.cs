// <copyright file="Course.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

/// <summary>
/// Represents a university course and its credit and pricing information.
/// </summary>
public class Course
{
    private readonly List<Prerequisite> prerequisites = new ();

    /// <summary>
    /// Initializes a new instance of the <see cref="Course"/> class.
    /// </summary>
    /// <param name="name">The course name.</param>
    /// <param name="description">The course description.</param>
    /// <param name="credits">The number of credits awarded by the course.</param>
    /// <param name="minimumCostPerCredit">The minimum cost for each credit.</param>
    /// <param name="cost">The total course cost.</param>
    /// <param name="currency">The currency used for the course cost.</param>
    public Course(
        string name,
        string description,
        int credits,
        decimal minimumCostPerCredit,
        decimal cost,
        string currency = "RON")
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
            throw new ArgumentException(
                "Minimum cost per credit must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException(
                "Course currency cannot be empty.",
                nameof(currency));
        }

        this.Name = name;
        this.Description = description;
        this.Credits = credits;
        this.MinimumCostPerCredit = minimumCostPerCredit;
        this.Cost = cost;
        this.Currency = currency;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Course"/> class with an identifier.
    /// </summary>
    /// <param name="id">The course identifier.</param>
    /// <param name="name">The course name.</param>
    /// <param name="description">The course description.</param>
    /// <param name="credits">The number of credits allocated to the course.</param>
    /// <param name="minimumCostPerCredit">The minimum cost per credit.</param>
    /// <param name="cost">The course cost.</param>
    /// <param name="currency">The currency used for the course cost.</param>
    public Course(
        int id,
        string name,
        string description,
        int credits,
        decimal minimumCostPerCredit,
        decimal cost,
        string currency = "RON")
        : this(
            name,
            description,
            credits,
            minimumCostPerCredit,
            cost,
            currency)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Course identifier must be greater than zero.",
                nameof(id));
        }

        this.Id = id;
    }

    /// <summary>
    /// Gets the persistent identifier of the course, when available.
    /// </summary>
    public int Id { get; private set; }

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

    /// <summary>
    /// Gets the currency used for the course cost.
    /// </summary>
    public string Currency { get; }

    /// <summary>
    /// Gets the prerequisites required for this course.
    /// </summary>
    public IReadOnlyCollection<Prerequisite> Prerequisites => this.prerequisites;

    /// <summary>
    /// Adds a prerequisite to the course.
    /// </summary>
    /// <param name="prerequisite">The prerequisite to add.</param>
    public void AddPrerequisite(Prerequisite prerequisite)
    {
        ArgumentNullException.ThrowIfNull(prerequisite);

        if (prerequisite.RequiredCourse == this)
        {
            throw new ArgumentException(
                "A course cannot be its own prerequisite.",
                nameof(prerequisite));
        }

        if (this.prerequisites.Exists(
            existingPrerequisite =>
                existingPrerequisite.RequiredCourse == prerequisite.RequiredCourse))
        {
            throw new ArgumentException(
                "The required course is already a prerequisite.",
                nameof(prerequisite));
        }

        this.prerequisites.Add(prerequisite);
    }
}
