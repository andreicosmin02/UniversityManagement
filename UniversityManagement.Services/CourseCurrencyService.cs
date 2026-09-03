// <copyright file="CourseCurrencyService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using System.Collections.Generic;
using System.Linq;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides validation for course currencies.
/// </summary>
public class CourseCurrencyService
{
    /// <summary>
    /// Validates that all supplied courses use the same currency.
    /// </summary>
    /// <param name="courses">The courses to validate.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the courses use different currencies.
    /// </exception>
    public void ValidateSameCurrency(IEnumerable<Course> courses)
    {
        ArgumentNullException.ThrowIfNull(courses);

        var currencyCount = courses
            .Select(course => course.Currency)
            .Distinct()
            .Take(2)
            .Count();

        if (currencyCount > 1)
        {
            throw new InvalidOperationException(
                "All courses must use the same currency.");
        }
    }
}
