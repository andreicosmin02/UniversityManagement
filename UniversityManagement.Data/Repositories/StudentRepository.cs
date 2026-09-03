// <copyright file="StudentRepository.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Repositories;

using UniversityManagement.Data.Persistence;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides persistence operations for students.
/// </summary>
public class StudentRepository
{
    private readonly UniversityManagementDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="StudentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public StudentRepository(UniversityManagementDbContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        this.context = context;
    }

    /// <summary>
    /// Adds and persists a student.
    /// </summary>
    /// <param name="student">The student to persist.</param>
    public void Add(Student student)
    {
        ArgumentNullException.ThrowIfNull(student);

        this.context.Students.Add(student);
        this.context.SaveChanges();
    }

    /// <summary>
    /// Gets a student by its persistent identifier.
    /// </summary>
    /// <param name="id">The student identifier.</param>
    /// <returns>The matching student, or <see langword="null"/> if none exists.</returns>
    public Student? GetById(int id)
    {
        return this.context.Students.Find(id);
    }
}
