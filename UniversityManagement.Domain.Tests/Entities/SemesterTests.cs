// <copyright file="SemesterTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Tests.Entities
{
    using UniversityManagement.Domain.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for the Semester entity.
    /// </summary>
    public class SemesterTests
    {
        /// <summary>
        /// Verifies that number and minimum credits are stored.
        /// </summary>
        [Fact]
        public void Semester_ShouldStoreNumberAndMinimumCredits()
        {
            var semester = new Semester(1, 30);

            Assert.Equal(1, semester.Number);
            Assert.Equal(30, semester.MinimumCredits);
        }

        /// <summary>
        /// Verifies non-positive semester numbers are rejected.
        /// </summary>
        /// <param name="number">The semester number to test.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Semester_ShouldRejectNonPositiveNumber(int number)
        {
            Assert.Throws<ArgumentException>(() =>
                new Semester(number, 30));
        }

        /// <summary>
        /// Verifies negative minimum credits are rejected.
        /// </summary>
        [Fact]
        public void Semester_ShouldRejectNegativeMinimumCredits()
        {
            Assert.Throws<ArgumentException>(() =>
                new Semester(1, -1));
        }

        /// <summary>
        /// Verifies zero minimum credits are accepted.
        /// </summary>
        [Fact]
        public void Semester_ShouldAcceptZeroMinimumCredits()
        {
            var semester = new Semester(1, 0);

            Assert.Equal(0, semester.MinimumCredits);
        }

        /// <summary>
        /// Verifies a course can be added to a semester.
        /// </summary>
        [Fact]
        public void Semester_ShouldAddCourse()
        {
            var semester = new Semester(1, 30);
            var course = new Course(
                "Programming",
                "Introduction to programming",
                5,
                100m,
                500m);

            semester.AddCourse(course);

            Assert.Contains(course, semester.Courses);
        }

        /// <summary>
        /// Verifies adding the same course twice is rejected.
        /// </summary>
        [Fact]
        public void Semester_ShouldRejectAddingSameCourseTwice()
        {
            var semester = new Semester(1, 30);
            var course = new Course(
                "Programming",
                "Introduction to programming",
                5,
                100m,
                500m);

            semester.AddCourse(course);

            Assert.Throws<ArgumentException>(() =>
                semester.AddCourse(course));
        }

        /// <summary>
        /// Verifies a course can belong to multiple semesters.
        /// </summary>
        [Fact]
        public void Course_ShouldBeAllowedInMultipleSemesters()
        {
            var firstSemester = new Semester(1, 30);
            var secondSemester = new Semester(2, 30);
            var course = new Course(
                "Programming",
                "Introduction to programming",
                5,
                100m,
                500m);

            firstSemester.AddCourse(course);
            secondSemester.AddCourse(course);

            Assert.Contains(course, firstSemester.Courses);
            Assert.Contains(course, secondSemester.Courses);
        }

        /// <summary>
        /// Verifies total credits are zero when no courses are present.
        /// </summary>
        [Fact]
        public void Semester_ShouldHaveZeroTotalCreditsWhenItHasNoCourses()
        {
            var semester = new Semester(1, 30);

            Assert.Equal(0, semester.TotalAvailableCredits);
        }

        /// <summary>
        /// Verifies calculation of total available credits.
        /// </summary>
        [Fact]
        public void Semester_ShouldCalculateTotalAvailableCredits()
        {
            var semester = new Semester(1, 30);

            var programming = new Course(
                "Programming",
                "Introduction to programming",
                5,
                100m,
                500m);

            var databases = new Course(
                "Databases",
                "Introduction to databases",
                6,
                100m,
                600m);

            semester.AddCourse(programming);
            semester.AddCourse(databases);

            Assert.Equal(11, semester.TotalAvailableCredits);
        }

        /// <summary>
        /// Verifies threshold behavior for minimum credits met.
        /// </summary>
        [Fact]
        public void Semester_ShouldMeetMinimumCreditsWhenAvailableCreditsReachThreshold()
        {
            var semester = new Semester(1, 10);

            var programming = new Course(
                "Programming",
                "Introduction to programming",
                5,
                100m,
                500m);

            var databases = new Course(
                "Databases",
                "Introduction to databases",
                5,
                100m,
                500m);

            semester.AddCourse(programming);
            semester.AddCourse(databases);

            Assert.True(semester.HasEnoughAvailableCredits);
        }

        /// <summary>
        /// Verifies when available credits are below threshold the flag is false.
        /// </summary>
        [Fact]
        public void Semester_ShouldNotMeetMinimumCreditsWhenAvailableCreditsAreBelowThreshold()
        {
            var semester = new Semester(1, 10);

            var programming = new Course(
                "Programming",
                "Introduction to programming",
                5,
                100m,
                500m);

            semester.AddCourse(programming);

            Assert.False(semester.HasEnoughAvailableCredits);
        }

        /// <summary>
        /// Verifies adding a null course throws ArgumentNullException.
        /// </summary>
        [Fact]
        public void Semester_ShouldRejectNullCourse()
        {
            var semester = new Semester(1, 30);

            Assert.Throws<ArgumentNullException>(() =>
                semester.AddCourse(null!));
        }

        /// <summary>
        /// Verifies that a new semester starts without a persistent identifier.
        /// </summary>
        [Fact]
        public void Semester_ShouldStartWithZeroId()
        {
            var semester = new Semester(1, 30);

            Assert.Equal(0, semester.Id);
        }

        /// <summary>
        /// Verifies that an existing semester can store a persistent identifier.
        /// </summary>
        [Fact]
        public void Semester_ShouldStorePositiveId()
        {
            var semester = new Semester(1, 1, 30);

            Assert.Equal(1, semester.Id);
        }

        /// <summary>
        /// Verifies that an existing semester rejects a non-positive identifier.
        /// </summary>
        /// <param name="id">The invalid identifier to test.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Semester_ShouldRejectNonPositiveId(int id)
        {
            Assert.Throws<ArgumentException>(() => new Semester(id, 1, 30));
        }
    }
}
