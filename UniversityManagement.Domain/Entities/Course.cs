namespace UniversityManagement.Domain.Entities;

public class Course
{
    public string Name { get; }
    public int Credits { get; }
    public string Description { get; }
    public decimal MinimumCostPerCredit { get; }
    public decimal Cost { get; }

    public Course(
    string name,
    string description,
    int credits,
    decimal minimumCostPerCredit,
    decimal cost)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Course name cannot be empty.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Course description cannot be empty.");

        if (credits <= 0)
            throw new ArgumentException("Course credits must be greater than zero.");

        decimal minimumCost = minimumCostPerCredit * credits;
        decimal maximumCost = 2 * minimumCostPerCredit * credits;

        if (cost < minimumCost || cost > maximumCost)
            throw new ArgumentException("Course cost is outside the allowed range.");

        if (minimumCostPerCredit <= 0)
            throw new ArgumentException("Minimum cost per credit must be greater than zero.");

        Name = name;
        Description = description;
        Credits = credits;
        MinimumCostPerCredit = minimumCostPerCredit;
        Cost = cost;
    }
}