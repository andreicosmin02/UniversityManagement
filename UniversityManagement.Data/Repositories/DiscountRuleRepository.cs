// <copyright file="DiscountRuleRepository.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides persistence operations for discount rules.
/// </summary>
public class DiscountRuleRepository
{
    private readonly UniversityManagementDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscountRuleRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public DiscountRuleRepository(UniversityManagementDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds and persists a discount rule.
    /// </summary>
    /// <param name="rule">The discount rule to persist.</param>
    public void Add(DiscountRule rule)
    {
        this.context.DiscountRules.Add(rule);
        this.context.SaveChanges();
    }

    /// <summary>
    /// Gets a discount rule and its courses by identifier.
    /// </summary>
    /// <param name="id">The discount rule identifier.</param>
    /// <returns>
    /// The matching discount rule, or <see langword="null"/> if none exists.
    /// </returns>
    public DiscountRule? GetById(int id)
    {
        return this.context.DiscountRules
            .Include(rule => rule.Courses)
            .SingleOrDefault(rule => rule.Id == id);
    }
}
