// <copyright file="ExamAttemptRepository.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Repositories;

using UniversityManagement.Data.Persistence;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides persistence operations for exam attempts.
/// </summary>
public class ExamAttemptRepository
{
    private readonly UniversityManagementDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExamAttemptRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public ExamAttemptRepository(UniversityManagementDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds and persists an exam attempt.
    /// </summary>
    /// <param name="attempt">The exam attempt to persist.</param>
    public void Add(ExamAttempt attempt)
    {
        this.context.ExamAttempts.Add(attempt);
        this.context.SaveChanges();
    }

    /// <summary>
    /// Gets an exam attempt by its persistent identifier.
    /// </summary>
    /// <param name="id">The exam attempt identifier.</param>
    /// <returns>
    /// The matching exam attempt, or <see langword="null"/> if none exists.
    /// </returns>
    public ExamAttempt? GetById(int id)
    {
        return this.context.ExamAttempts.Find(id);
    }
}
