// <copyright file="ExamAttempt.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

/// <summary>
/// Represents a student's attempt to take an exam for a course.
/// </summary>
public class ExamAttempt
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExamAttempt"/> class.
    /// </summary>
    /// <param name="course">The course for which the exam is taken.</param>
    /// <param name="grade">The grade obtained at the exam.</param>
    /// <param name="examDate">The date when the examination took place.</param>
    public ExamAttempt(
        Course course,
        int grade,
        DateTime examDate)
    {
        ArgumentNullException.ThrowIfNull(course);

        if (grade < 1 || grade > 10)
        {
            throw new ArgumentException(
                "Grade must be between 1 and 10.",
                nameof(grade));
        }

        this.Course = course;
        this.Grade = grade;
        this.ExamDate = examDate;
    }

    /// <summary>
    /// Gets the course for which the exam was taken.
    /// </summary>
    public Course Course { get; }

    /// <summary>
    /// Gets the grade obtained at the exam.
    /// </summary>
    public int Grade { get; }

    /// <summary>
    /// Gets the examination date.
    /// </summary>
    public DateTime ExamDate { get; }

    /// <summary>
    /// Gets a value indicating whether the exam was passed.
    /// </summary>
    public bool Passed => this.Grade >= 5;
}
