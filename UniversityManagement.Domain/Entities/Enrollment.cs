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
    /// Initializes a new instance of the <see cref="Enrollment"/> class
    /// with an existing persistent identifier.
    /// </summary>
    /// <param name="id">The persistent identifier.</param>
    /// <param name="student">The enrolled student.</param>
    /// <param name="course">The selected course.</param>
    /// <param name="semester">The semester of the enrollment.</param>
    public Enrollment(
        int id,
        Student student,
        Course course,
        Semester semester)
        : this(student, course, semester)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Enrollment identifier must be greater than zero.",
                nameof(id));
        }

        this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enrollment"/> class for persistence.
    /// </summary>
    private Enrollment()
    {
        this.Student = null!;
        this.Course = null!;
        this.Semester = null!;
    }

    /// <summary>
    /// Gets the persistent identifier of the enrollment.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the enrolled student.
    /// </summary>
    public Student Student { get; private set; }

    /// <summary>
    /// Gets the selected course.
    /// </summary>
    public Course Course { get; private set; }

    /// <summary>
    /// Gets the semester of the enrollment.
    /// </summary>
    public Semester Semester { get; private set; }
}
