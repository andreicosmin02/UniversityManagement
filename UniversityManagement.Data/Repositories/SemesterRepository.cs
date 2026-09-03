// <copyright file="SemesterRepository.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides persistence operations for semesters.
/// </summary>
public class SemesterRepository
{
    private readonly UniversityManagementDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SemesterRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SemesterRepository(UniversityManagementDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds and persists a semester.
    /// </summary>
    /// <param name="semester">The semester to persist.</param>
    public void Add(Semester semester)
    {
        this.context.Semesters.Add(semester);
        this.context.SaveChanges();
    }

    /// <summary>
    /// Gets a semester by its persistent identifier.
    /// </summary>
    /// <param name="id">The semester identifier.</param>
    /// <returns>The matching semester, or <see langword="null"/> if none exists.</returns>
    public Semester? GetById(int id)
    {
        return this.context.Semesters
            .Include(semester => semester.Courses)
            .SingleOrDefault(semester => semester.Id == id);
    }
}
