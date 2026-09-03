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

        /// <summary>
        /// Verifies that a prerequisite can be added to a course.
        /// </summary>
        [Fact]
        public void Course_ShouldAddPrerequisite()
        {
            var requiredCourse = new Course(
                "Programare",
                "Descriere",
                5,
                100m,
                750m);

            var course = new Course(
                "Algoritmi",
                "Descriere",
                5,
                100m,
                750m);

            var prerequisite = new Prerequisite(requiredCourse, 7);

            course.AddPrerequisite(prerequisite);

            Assert.Contains(prerequisite, course.Prerequisites);
        }

        /// <summary>
        /// Verifies that the same prerequisite course cannot be added twice.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectDuplicatePrerequisite()
        {
            var requiredCourse = new Course(
                "Programare",
                "Descriere",
                5,
                100m,
                750m);

            var course = new Course(
                "Algoritmi",
                "Descriere",
                5,
                100m,
                750m);

            var prerequisite = new Prerequisite(requiredCourse, 7);

            course.AddPrerequisite(prerequisite);

            Assert.Throws<ArgumentException>(
                () => course.AddPrerequisite(prerequisite));
        }

        /// <summary>
        /// Verifies that a null prerequisite cannot be added to a course.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectNullPrerequisite()
        {
            var course = new Course(
                "Algoritmi",
                "Descriere",
                5,
                100m,
                750m);

            Assert.Throws<ArgumentNullException>(
                () => course.AddPrerequisite(null!));
        }

        /// <summary>
        /// Verifies that a course cannot require itself as a prerequisite.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectItselfAsPrerequisite()
        {
            var course = new Course(
                "Algoritmi",
                "Descriere",
                5,
                100m,
                750m);

            var prerequisite = new Prerequisite(course, 7);

            Assert.Throws<ArgumentException>(
                () => course.AddPrerequisite(prerequisite));
        }

        /// <summary>
        /// Verifies that two prerequisite objects for the same required course are rejected.
        /// </summary>
        [Fact]
        public void Course_ShouldRejectDuplicateRequiredCourse()
        {
            var requiredCourse = new Course(
                "Programare",
                "Descriere",
                5,
                100m,
                750m);

            var course = new Course(
                "Algoritmi",
                "Descriere",
                5,
                100m,
                750m);

            var firstPrerequisite = new Prerequisite(requiredCourse, 5);
            var secondPrerequisite = new Prerequisite(requiredCourse, 7);

            course.AddPrerequisite(firstPrerequisite);

            Assert.Throws<ArgumentException>(
                () => course.AddPrerequisite(secondPrerequisite));
        }

        /// <summary>
        /// Verifies that different prerequisite courses can be added.
        /// </summary>
        [Fact]
        public void Course_ShouldAllowDifferentPrerequisiteCourses()
        {
            var programming = new Course(
                "Programare",
                "Descriere",
                5,
                100m,
                750m);

            var mathematics = new Course(
                "Matematica",
                "Descriere",
                5,
                100m,
                750m);

            var course = new Course(
                "Algoritmi",
                "Descriere",
                5,
                100m,
                750m);

            var programmingPrerequisite = new Prerequisite(programming, 7);
            var mathematicsPrerequisite = new Prerequisite(mathematics, 6);

            course.AddPrerequisite(programmingPrerequisite);
            course.AddPrerequisite(mathematicsPrerequisite);

            Assert.Equal(2, course.Prerequisites.Count);
        }

        /// <summary>
        /// Verifies that a course stores its identifier.
        /// </summary>
        [Fact]
        public void Course_ShouldStoreId()
        {
            var course = new Course(
                42,
                "Programming",
                "Introduction to programming.",
                5,
                100m,
                500m);

            Assert.Equal(42, course.Id);
        }

        /// <summary>
        /// Verifies that a course rejects a non-positive identifier.
        /// </summary>
        /// <param name="id">The identifier to test.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Course_ShouldRejectNonPositiveId(int id)
        {
            Assert.Throws<ArgumentException>(() =>
                new Course(
                    id,
                    "Programming",
                    "Introduction to programming.",
                    5,
                    100m,
                    500m));
        }

        /// <summary>
        /// Verifies that a new course starts without an assigned persistent identifier.
        /// </summary>
        [Fact]
        public void Course_ShouldStartWithUnassignedId()
        {
            var course = new Course(
                "Programming",
                "Introduction to programming.",
                5,
                100m,
                500m);

            Assert.Equal(0, course.Id);
        }

        /// <summary>
        /// Verifies that a course stores its currency.
        /// </summary>
        [Fact]
        public void Course_ShouldStoreCurrency()
        {
            var course = new Course(
                "Programming",
                "Introduction to programming.",
                5,
                100m,
                500m,
                "EUR");

            Assert.Equal("EUR", course.Currency);
        }

        /// <summary>
        /// Verifies that an invalid currency is rejected.
        /// </summary>
        /// <param name="currency">The invalid currency value.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Course_ShouldRejectInvalidCurrency(string currency)
        {
            Assert.Throws<ArgumentException>(() =>
                new Course(
                    "Programming",
                    "Introduction to programming.",
                    5,
                    100m,
                    500m,
                    currency));
        }
    }
}
