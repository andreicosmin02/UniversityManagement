// <copyright file="CourseRepository.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Repositories;

using UniversityManagement.Data.Persistence;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides persistence operations for courses.
/// </summary>
public class CourseRepository
{
    private readonly UniversityManagementDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CourseRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for persistence.</param>
    public CourseRepository(UniversityManagementDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds a course to the database.
    /// </summary>
    /// <param name="course">The course to add.</param>
    public void Add(Course course)
    {
        this.context.Courses.Add(course);
        this.context.SaveChanges();
    }

    /// <summary>
    /// Gets a course by its identifier.
    /// </summary>
    /// <param name="id">The course identifier.</param>
    /// <returns>The course, or <see langword="null"/> if it does not exist.</returns>
    public Course? GetById(int id)
    {
        return this.context.Courses.Find(id);
    }
}
