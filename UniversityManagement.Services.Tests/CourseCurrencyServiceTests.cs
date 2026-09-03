// <copyright file="CourseCurrencyServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests course currency validation.
/// </summary>
public class CourseCurrencyServiceTests
{
    /// <summary>
    /// Verifies that courses using the same currency are accepted.
    /// </summary>
    [Fact]
    public void ValidateSameCurrency_ShouldAcceptCoursesWithSameCurrency()
    {
        var firstCourse = new Course(
            "A",
            "Course A",
            5,
            100m,
            500m,
            "RON");

        var secondCourse = new Course(
            "B",
            "Course B",
            5,
            100m,
            600m,
            "RON");

        var service = new CourseCurrencyService();

        var exception = Record.Exception(() =>
            service.ValidateSameCurrency(
                new[] { firstCourse, secondCourse }));

        Assert.Null(exception);
    }

    /// <summary>
    /// Verifies that courses using different currencies are rejected.
    /// </summary>
    [Fact]
    public void ValidateSameCurrency_ShouldRejectDifferentCurrencies()
    {
        var firstCourse = new Course(
            "A",
            "Course A",
            5,
            100m,
            500m,
            "RON");

        var secondCourse = new Course(
            "B",
            "Course B",
            5,
            100m,
            600m,
            "EUR");

        var service = new CourseCurrencyService();

        Assert.Throws<InvalidOperationException>(() =>
            service.ValidateSameCurrency(
                new[] { firstCourse, secondCourse }));
    }
}
