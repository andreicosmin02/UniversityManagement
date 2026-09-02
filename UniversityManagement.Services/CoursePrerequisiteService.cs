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
    /// Adds a prerequisite to a course when it does not create a circular dependency.
    /// </summary>
    /// <param name="course">The course that receives the prerequisite.</param>
    /// <param name="prerequisite">The prerequisite to add.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when adding the prerequisite would create a circular dependency.
    /// </exception>
    public void AddPrerequisite(Course course, Prerequisite prerequisite)
    {
        ArgumentNullException.ThrowIfNull(course);
        ArgumentNullException.ThrowIfNull(prerequisite);

        if (HasPathToCourse(prerequisite.RequiredCourse, course))
        {
            throw new InvalidOperationException(
                "Adding this prerequisite would create a circular dependency.");
        }

        course.AddPrerequisite(prerequisite);
    }

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

        var courseSemesters = semesters
            .Where(semester => semester.Courses.Contains(course));

        if (!courseSemesters.Any())
        {
            throw new InvalidOperationException(
                "The course must be assigned to at least one semester.");
        }

        var requiredCourseSemesters = semesters
            .Where(semester => semester.Courses.Contains(prerequisite.RequiredCourse));

        if (courseSemesters.Any(
            courseSemester => !requiredCourseSemesters.Any(
                requiredSemester => requiredSemester.Number < courseSemester.Number)))
        {
            throw new InvalidOperationException(
                "The prerequisite course must be available in an earlier semester.");
        }

        this.AddPrerequisite(course, prerequisite);
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
