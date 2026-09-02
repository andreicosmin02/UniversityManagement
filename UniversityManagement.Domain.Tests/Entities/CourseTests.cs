// <copyright file="CourseTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Tests.Entities
{
    using UniversityManagement.Domain.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for the Course entity.
    /// </summary>
    public class CourseTests
    {
        /// <summary>
        /// Verifies that the course stores the provided name.
        /// </summary>
        [Fact]
        public void Course_ShouldStoreName()
        {
            var course = new Course("Programare", "Descriere", 5, 100m, 750m);

            Assert.Equal("Programare", course.Name);
        }

        /// <summary>
        /// Verifies that empty course name is rejected.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectEmptyName()
        {
            Assert.Throws<ArgumentException>(
                () => new Course(string.Empty, "Descriere", 5, 100m, 750m));
        }

        /// <summary>
        /// Verifies that whitespace-only course name is rejected.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectWhitespaceName()
        {
            Assert.Throws<ArgumentException>(() => new Course("   ", "Descriere", 5, 100m, 750m));
        }

        /// <summary>
        /// Verifies that the course stores credits.
        /// </summary>
        [Fact]
        public void Course_ShouldStoreCredits()
        {
            var course = new Course("Programare", "Descriere", 5, 100m, 750m);

            Assert.Equal(5, course.Credits);
        }

        /// <summary>
        /// Verifies that non-positive credits are rejected.
        /// </summary>
        /// <param name="credits">The credit value to test.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Course_ShouldRejectNonPositiveCredits(int credits)
        {
            Assert.Throws<ArgumentException>(
                () => new Course("Programare", "Descriere", credits, 100m, 750m));
        }

        /// <summary>
        /// Verifies that the course stores the description.
        /// </summary>
        [Fact]
        public void Course_ShouldStoreDescription()
        {
            var course = new Course("Programare", "Introducere in C#", 5, 100m, 750m);

            Assert.Equal("Introducere in C#", course.Description);
        }

        /// <summary>
        /// Verifies that invalid descriptions are rejected.
        /// </summary>
        /// <param name="description">The description value to test.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Course_ShouldRejectInvalidDescription(string description)
        {
            Assert.Throws<ArgumentException>(
                () => new Course("Programare", description, 5, 100m, 750m));
        }

        /// <summary>
        /// Verifies cost and minimum cost per credit are stored.
        /// </summary>
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

        /// <summary>
        /// Verifies cost below minimum is rejected.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectCostBelowMinimum()
        {
            Assert.Throws<ArgumentException>(() =>
                new Course("Programare", "Descriere", 5, 100m, 499m));
        }

        /// <summary>
        /// Verifies cost above maximum is rejected.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectCostAboveMaximum()
        {
            Assert.Throws<ArgumentException>(() =>
                new Course("Programare", "Descriere", 5, 100m, 1001m));
        }

        /// <summary>
        /// Verifies zero minimum cost per credit is rejected.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectZeroMinimumCostPerCredit()
        {
            Assert.Throws<ArgumentException>(() =>
                new Course("Programare", "Descriere", 5, 0m, 0m));
        }

        /// <summary>
        /// Verifies cost at allowed boundaries is accepted.
        /// </summary>
        /// <param name="cost">The cost value to test.</param>
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
}
