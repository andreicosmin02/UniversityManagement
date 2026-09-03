// <copyright file="CoursePrerequisiteService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides operations for managing course prerequisites.
/// </summary>
public class CoursePrerequisiteService
{
    /// <summary>
    /// Adds a prerequisite when the required course is available in an earlier semester.
    /// </summary>
    /// <param name="course">The course that receives the prerequisite.</param>
    /// <param name="prerequisite">The prerequisite to add.</param>
    /// <param name="semesters">The semesters containing the available courses.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the required course is not available before a semester containing the course.
    /// </exception>
    public void AddPrerequisite(
        Course course,
        Prerequisite prerequisite,
        IEnumerable<Semester> semesters)
    {
        ArgumentNullException.ThrowIfNull(course);
        ArgumentNullException.ThrowIfNull(prerequisite);
        ArgumentNullException.ThrowIfNull(semesters);

        var semesterList = semesters.ToList();

        var courseSemesters = semesterList
            .Where(semester => semester.Courses.Contains(course))
            .ToList();

        if (courseSemesters.Count == 0)
        {
            throw new InvalidOperationException(
                "The course must be assigned to at least one semester.");
        }

        if (courseSemesters.Any(semester => semester.Number == 1))
        {
            throw new InvalidOperationException(
                "A course offered in the first semester cannot have prerequisites.");
        }

        var requiredCourseSemesters = semesterList
            .Where(semester => semester.Courses.Contains(prerequisite.RequiredCourse))
            .ToList();

        if (courseSemesters.Any(
            courseSemester => !requiredCourseSemesters.Any(
                requiredSemester => requiredSemester.Number < courseSemester.Number)))
        {
            throw new InvalidOperationException(
                "The prerequisite course must be available in an earlier semester.");
        }

        AddPrerequisiteAfterCycleCheck(course, prerequisite);
    }

    /// <summary>
    /// Adds a prerequisite after validating that it does not create a cycle.
    /// </summary>
    /// <param name="course">The course that receives the prerequisite.</param>
    /// <param name="prerequisite">The prerequisite to add.</param>
    private static void AddPrerequisiteAfterCycleCheck(
        Course course,
        Prerequisite prerequisite)
    {
        if (HasPathToCourse(prerequisite.RequiredCourse, course))
        {
            throw new InvalidOperationException(
                "Adding this prerequisite would create a circular dependency.");
        }

        course.AddPrerequisite(prerequisite);
    }

    /// <summary>
    /// Determines whether a prerequisite path reaches the specified course.
    /// </summary>
    /// <param name="currentCourse">The course from which the search starts.</param>
    /// <param name="targetCourse">The course being searched for.</param>
    /// <returns>
    /// <see langword="true"/> when the target course can be reached; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    private static bool HasPathToCourse(Course currentCourse, Course targetCourse)
    {
        if (ReferenceEquals(currentCourse, targetCourse))
        {
            return true;
        }

        foreach (var prerequisite in currentCourse.Prerequisites)
        {
            if (HasPathToCourse(prerequisite.RequiredCourse, targetCourse))
            {
                return true;
            }
        }

        return false;
    }
}
