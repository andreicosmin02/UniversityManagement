// <copyright file="PrerequisiteTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Tests.Entities
{
    using UniversityManagement.Domain.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for the Prerequisite entity.
    /// </summary>
    public class PrerequisiteTests
    {
        /// <summary>
        /// Verifies that a prerequisite stores its required course and minimum grade.
        /// </summary>
        [Fact]
        public void Prerequisite_ShouldStoreRequiredCourseAndMinimumGrade()
        {
            var requiredCourse = new Course(
                "Programare",
                "Descriere",
                5,
                100m,
                750m);

            var prerequisite = new Prerequisite(requiredCourse, 7);

            Assert.Same(requiredCourse, prerequisite.RequiredCourse);
            Assert.Equal(7, prerequisite.MinimumGrade);
        }

        /// <summary>
        /// Verifies that a prerequisite cannot be created without a required course.
        /// </summary>
        [Fact]
        public void Prerequisite_ShouldRejectNullRequiredCourse()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Prerequisite(null!, 7));
        }

        /// <summary>
        /// Verifies that prerequisite grades outside the valid range are rejected.
        /// </summary>
        /// <param name="minimumGrade">The minimum grade to test.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(11)]
        public void Prerequisite_ShouldRejectMinimumGradeOutsideValidRange(int minimumGrade)
        {
            var requiredCourse = new Course(
                "Programare",
                "Descriere",
                5,
                100m,
                750m);

            Assert.Throws<ArgumentException>(
                () => new Prerequisite(requiredCourse, minimumGrade));
        }

        /// <summary>
        /// Verifies that grades at the valid boundaries are accepted.
        /// </summary>
        /// <param name="minimumGrade">The minimum grade to test.</param>
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        public void Prerequisite_ShouldAcceptMinimumGradeAtValidBoundaries(int minimumGrade)
        {
            var requiredCourse = new Course(
                "Programare",
                "Descriere",
                5,
                100m,
                750m);

            var prerequisite = new Prerequisite(requiredCourse, minimumGrade);

            Assert.Equal(minimumGrade, prerequisite.MinimumGrade);
        }

        /// <summary>
        /// Verifies that a new prerequisite starts without a persistent identifier.
        /// </summary>
        [Fact]
        public void Prerequisite_ShouldStartWithZeroId()
        {
            var course = new Course("Math", "Mathematics", 5, 100, 500);

            var prerequisite = new Prerequisite(course, 7);

            Assert.Equal(0, prerequisite.Id);
        }

        /// <summary>
        /// Verifies that an existing prerequisite can store a persistent identifier.
        /// </summary>
        [Fact]
        public void Prerequisite_ShouldStorePositiveId()
        {
            var course = new Course("Math", "Mathematics", 5, 100, 500);

            var prerequisite = new Prerequisite(1, course, 7);

            Assert.Equal(1, prerequisite.Id);
        }

        /// <summary>
        /// Verifies that an existing prerequisite rejects a non-positive identifier.
        /// </summary>
        /// <param name="id">The invalid identifier to test.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Prerequisite_ShouldRejectNonPositiveId(int id)
        {
            var course = new Course("Math", "Mathematics", 5, 100, 500);

            Assert.Throws<ArgumentException>(
                () => new Prerequisite(id, course, 7));
        }
    }
}
