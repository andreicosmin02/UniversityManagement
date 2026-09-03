// <copyright file="EnrollmentRepository.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides persistence operations for enrollments.
/// </summary>
public class EnrollmentRepository
{
    private readonly UniversityManagementDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="EnrollmentRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public EnrollmentRepository(UniversityManagementDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds and persists an enrollment.
    /// </summary>
    /// <param name="enrollment">The enrollment to persist.</param>
    public void Add(Enrollment enrollment)
    {
        this.context.Enrollments.Add(enrollment);
        this.context.SaveChanges();
    }

    /// <summary>
    /// Gets an enrollment by its persistent identifier.
    /// </summary>
    /// <param name="id">The enrollment identifier.</param>
    /// <returns>
    /// The matching enrollment, or <see langword="null"/> if none exists.
    /// </returns>
    public Enrollment? GetById(int id)
    {
        return this.context.Enrollments
            .Include(enrollment => enrollment.Student)
            .Include(enrollment => enrollment.Course)
            .Include(enrollment => enrollment.Semester)
            .SingleOrDefault(enrollment => enrollment.Id == id);
    }
}
