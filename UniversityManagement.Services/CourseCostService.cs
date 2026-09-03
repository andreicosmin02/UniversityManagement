// <copyright file="CourseCostService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using System.Collections.Generic;
using System.Linq;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides operations for calculating course costs.
/// </summary>
public class CourseCostService
{
    /// <summary>
    /// Calculates the total cost of the selected courses and applies the discount
    /// when the complete course combination is selected.
    /// </summary>
    /// <param name="selectedCourses">The selected courses.</param>
    /// <param name="discountRule">The discount rule to evaluate.</param>
    /// <returns>The total cost after applying any eligible discount.</returns>
    public decimal CalculateTotal(
        IEnumerable<Course> selectedCourses,
        DiscountRule discountRule)
    {
        ArgumentNullException.ThrowIfNull(selectedCourses);
        ArgumentNullException.ThrowIfNull(discountRule);

        var selectedCourseList = selectedCourses.ToList();

        var discountApplies = discountRule.Courses.All(
            course => selectedCourseList.Contains(course));

        var total = 0m;

        foreach (var course in selectedCourseList)
        {
            var courseCost = course.Cost;

            if (discountApplies && discountRule.Courses.Contains(course))
            {
                courseCost -= courseCost * discountRule.Percentage / 100m;
            }

            total += courseCost;
        }

        return total;
    }
}
