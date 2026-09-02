// <copyright file="Enrollment.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

/// <summary>
/// Represents a student's selection of a course in a semester.
/// </summary>
public class Enrollment
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Enrollment"/> class.
    /// </summary>
    /// <param name="student">The enrolled student.</param>
    /// <param name="course">The selected course.</param>
    /// <param name="semester">The semester in which the course is selected.</param>
    public Enrollment(Student student, Course course, Semester semester)
    {
        this.Student = student;
        this.Course = course;
        this.Semester = semester;
    }

    /// <summary>
    /// Gets the enrolled student.
    /// </summary>
    public Student Student { get; }

    /// <summary>
    /// Gets the selected course.
    /// </summary>
    public Course Course { get; }

    /// <summary>
    /// Gets the semester of the enrollment.
    /// </summary>
    public Semester Semester { get; }
}
