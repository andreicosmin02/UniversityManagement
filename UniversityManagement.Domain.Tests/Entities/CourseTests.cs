using UniversityManagement.Domain.Entities;
using Xunit;

namespace UniversityManagement.Domain.Tests.Entities;

public class CourseTests
{
    [Fact]
    public void Course_ShouldStoreName()
    {
        var course = new Course("Programare", "Descriere", 5, 100m, 750m);

        Assert.Equal("Programare", course.Name);
    }

    [Fact]
    public void Course_ShouldRejectEmptyName()
    {
        Assert.Throws<ArgumentException>(
            () => new Course("", "Descriere", 5, 100m, 750m));
    }

    [Fact]
    public void Course_ShouldRejectWhitespaceName()
    {
        Assert.Throws<ArgumentException>(() => new Course("   ", "Descriere", 5, 100m, 750m));
    }

    [Fact]
    public void Course_ShouldStoreCredits()
    {
        var course = new Course("Programare", "Descriere", 5, 100m, 750m);

        Assert.Equal(5, course.Credits);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Course_ShouldRejectNonPositiveCredits(int credits)
    {
        Assert.Throws<ArgumentException>(
            () => new Course("Programare", "Descriere", credits, 100m, 750m));
    }

    [Fact]
    public void Course_ShouldStoreDescription()
    {
        var course = new Course("Programare", "Introducere in C#", 5, 100m, 750m);

        Assert.Equal("Introducere in C#", course.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Course_ShouldRejectInvalidDescription(string description)
    {
        Assert.Throws<ArgumentException>(
            () => new Course("Programare", description, 5, 100m, 750m));
    }

    [Fact]
    public void Course_ShouldStoreCostAndMinimumCostPerCredit()
    {
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            750m);

        Assert.Equal(100m, course.MinimumCostPerCredit);
        Assert.Equal(750m, course.Cost);
    }

    [Fact]
    public void Course_ShouldRejectCostBelowMinimum()
    {
        Assert.Throws<ArgumentException>(() =>
            new Course("Programare", "Descriere", 5, 100m, 499m));
    }

    [Fact]
    public void Course_ShouldRejectCostAboveMaximum()
    {
        Assert.Throws<ArgumentException>(() =>
            new Course("Programare", "Descriere", 5, 100m, 1001m));
    }

    [Fact]
    public void Course_ShouldRejectZeroMinimumCostPerCredit()
    {
        Assert.Throws<ArgumentException>(() =>
            new Course("Programare", "Descriere", 5, 0m, 0m));
    }

    [Theory]
    [InlineData(500)]
    [InlineData(1000)]
    public void Course_ShouldAcceptCostAtAllowedBoundaries(decimal cost)
    {
        var course = new Course(
            "Programare",
            "Descriere",
            5,
            100m,
            cost);

        Assert.Equal(cost, course.Cost);
    }
}
