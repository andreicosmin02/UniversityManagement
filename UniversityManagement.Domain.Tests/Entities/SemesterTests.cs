using UniversityManagement.Domain.Entities;
using Xunit;

namespace UniversityManagement.Domain.Tests.Entities;

public class SemesterTests
{
    [Fact]
    public void Semester_ShouldStoreNumberAndMinimumCredits()
    {
        var semester = new Semester(1, 30);

        Assert.Equal(1, semester.Number);
        Assert.Equal(30, semester.MinimumCredits);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Semester_ShouldRejectNonPositiveNumber(int number)
    {
        Assert.Throws<ArgumentException>(() =>
            new Semester(number, 30));
    }

    [Fact]
    public void Semester_ShouldRejectNegativeMinimumCredits()
    {
        Assert.Throws<ArgumentException>(() =>
            new Semester(1, -1));
    }

    [Fact]
    public void Semester_ShouldAcceptZeroMinimumCredits()
    {
        var semester = new Semester(1, 0);

        Assert.Equal(0, semester.MinimumCredits);
    }

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


    [Fact]
    public void Semester_ShouldHaveZeroTotalCreditsWhenItHasNoCourses()
    {
        var semester = new Semester(1, 30);

        Assert.Equal(0, semester.TotalAvailableCredits);
    }

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
}