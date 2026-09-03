// <copyright file="CoursePrerequisiteServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests prerequisite operations performed by the service layer.
/// </summary>
public class CoursePrerequisiteServiceTests
{
    /// <summary>
    /// Verifies that a circular dependency between two courses is rejected.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldRejectTwoCourseCircularDependency()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        courseA.AddPrerequisite(new Prerequisite(courseB, 5));
        var semesterOne = new Semester(1, 0);
        semesterOne.AddCourse(courseA);
        var semesterTwo = new Semester(2, 0);
        semesterTwo.AddCourse(courseB);

        var service = new CoursePrerequisiteService();

        Assert.Throws<InvalidOperationException>(
            () => service.AddPrerequisite(
                courseB,
                new Prerequisite(courseA, 5),
                new[] { semesterOne, semesterTwo }));
    }

    /// <summary>
    /// Verifies that a circular dependency involving three courses is rejected.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldRejectThreeCourseCircularDependency()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);
        var courseC = new Course("C", "Course C", 5, 100m, 500m);

        courseA.AddPrerequisite(new Prerequisite(courseB, 5));
        courseB.AddPrerequisite(new Prerequisite(courseC, 5));
        var semesterOne = new Semester(1, 0);
        semesterOne.AddCourse(courseA);
        var semesterTwo = new Semester(2, 0);
        semesterTwo.AddCourse(courseB);
        var semesterThree = new Semester(3, 0);
        semesterThree.AddCourse(courseC);

        var service = new CoursePrerequisiteService();

        Assert.Throws<InvalidOperationException>(
            () => service.AddPrerequisite(
                courseC,
                new Prerequisite(courseA, 5),
                new[] { semesterOne, semesterTwo, semesterThree }));
    }

    /// <summary>
    /// Verifies that a prerequisite is added when it does not create a cycle.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldAddPrerequisiteWhenDependencyIsAcyclic()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);
        var courseC = new Course("C", "Course C", 5, 100m, 500m);

        courseA.AddPrerequisite(new Prerequisite(courseB, 5));
        var semesterOne = new Semester(1, 0);
        semesterOne.AddCourse(courseA);
        var semesterTwo = new Semester(2, 0);
        semesterTwo.AddCourse(courseC);

        var prerequisite = new Prerequisite(courseA, 5);
        var service = new CoursePrerequisiteService();

        service.AddPrerequisite(
            courseC,
            prerequisite,
            new[] { semesterOne, semesterTwo });

        Assert.Contains(prerequisite, courseC.Prerequisites);
    }

    /// <summary>
    /// Verifies that a null course is rejected.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldRejectNullCourse()
    {
        var requiredCourse = new Course("B", "Course B", 5, 100m, 500m);
        var prerequisite = new Prerequisite(requiredCourse, 5);
        var semester = new Semester(1, 0);
        semester.AddCourse(requiredCourse);
        var service = new CoursePrerequisiteService();

        Assert.Throws<ArgumentNullException>(
            () => service.AddPrerequisite(
                null!,
                prerequisite,
                new[] { semester }));
    }

    /// <summary>
    /// Verifies that a null prerequisite is rejected.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldRejectNullPrerequisite()
    {
        var course = new Course("A", "Course A", 5, 100m, 500m);
        var semester = new Semester(2, 0);
        semester.AddCourse(course);
        var service = new CoursePrerequisiteService();

        Assert.Throws<ArgumentNullException>(
            () => service.AddPrerequisite(
                course,
                null!,
                new[] { semester }));
    }

    /// <summary>
    /// Verifies that a prerequisite course must be available in an earlier semester.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldRejectRequiredCourseFromSameSemester()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var semester = new Semester(2, 0);
        semester.AddCourse(courseA);
        semester.AddCourse(courseB);

        var prerequisite = new Prerequisite(courseA, 5);
        var service = new CoursePrerequisiteService();

        Assert.Throws<InvalidOperationException>(
            () => service.AddPrerequisite(
                courseB,
                prerequisite,
                new[] { semester }));
    }

    /// <summary>
    /// Verifies that a course not assigned to any semester cannot receive a prerequisite.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldRejectCourseNotAssignedToAnySemester()
    {
        var courseA = new Course("A", "Course A", 5, 100m, 500m);
        var courseB = new Course("B", "Course B", 5, 100m, 500m);

        var semester = new Semester(1, 0);
        semester.AddCourse(courseA);

        var prerequisite = new Prerequisite(courseA, 5);
        var service = new CoursePrerequisiteService();

        Assert.Throws<InvalidOperationException>(
            () => service.AddPrerequisite(
                courseB,
                prerequisite,
                new[] { semester }));
    }

    /// <summary>
    /// Verifies that a null course is rejected when semester validation is used.
    /// </summary>
    [Fact]
    public void AddPrerequisite_WithSemesters_ShouldRejectNullCourse()
    {
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var prerequisite = new Prerequisite(requiredCourse, 5);
        var semester = new Semester(1, 0);
        semester.AddCourse(requiredCourse);

        var service = new CoursePrerequisiteService();

        Assert.Throws<ArgumentNullException>(
            () => service.AddPrerequisite(
                null!,
                prerequisite,
                new[] { semester }));
    }

    /// <summary>
    /// Verifies that a null prerequisite is rejected when semester validation is used.
    /// </summary>
    [Fact]
    public void AddPrerequisite_WithSemesters_ShouldRejectNullPrerequisite()
    {
        var course = new Course("B", "Course B", 5, 100m, 500m);
        var semester = new Semester(2, 0);
        semester.AddCourse(course);

        var service = new CoursePrerequisiteService();

        Assert.Throws<ArgumentNullException>(
            () => service.AddPrerequisite(
                course,
                null!,
                new[] { semester }));
    }

    /// <summary>
    /// Verifies that a course in the first semester cannot have prerequisites.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldRejectPrerequisiteForFirstSemesterCourse()
    {
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        var semester = new Semester(1, 0);
        semester.AddCourse(requiredCourse);
        semester.AddCourse(course);
        var service = new CoursePrerequisiteService();

        Assert.Throws<InvalidOperationException>(
            () => service.AddPrerequisite(
                course,
                new Prerequisite(requiredCourse, 5),
                new[] { semester }));
    }

    /// <summary>
    /// Verifies that an earlier-semester course is accepted as a prerequisite.
    /// </summary>
    [Fact]
    public void AddPrerequisite_ShouldAcceptRequiredCourseFromEarlierSemester()
    {
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        var semesterOne = new Semester(1, 0);
        semesterOne.AddCourse(requiredCourse);
        var semesterTwo = new Semester(2, 0);
        semesterTwo.AddCourse(course);
        var prerequisite = new Prerequisite(requiredCourse, 5);
        var service = new CoursePrerequisiteService();

        service.AddPrerequisite(
            course,
            prerequisite,
            new[] { semesterOne, semesterTwo });

        Assert.Contains(prerequisite, course.Prerequisites);
    }

    /// <summary>
    /// Verifies that contextual prerequisite validation rejects null semesters.
    /// </summary>
    [Fact]
    public void AddPrerequisite_WithSemesters_ShouldRejectNullSemesters()
    {
        var requiredCourse = new Course("A", "Course A", 5, 100m, 500m);
        var course = new Course("B", "Course B", 5, 100m, 500m);
        var prerequisite = new Prerequisite(requiredCourse, 5);
        var service = new CoursePrerequisiteService();

        Assert.Throws<ArgumentNullException>(
            () => service.AddPrerequisite(course, prerequisite, null!));
    }
}
