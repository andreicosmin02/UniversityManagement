// <copyright file="FinancialReportingServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests financial reporting operations.
/// </summary>
public class FinancialReportingServiceTests
{
    /// <summary>
    /// Verifies that payments and refunds are included in the student's total.
    /// </summary>
    [Fact]
    public void GetTotalPaid_ShouldIncludePaymentsAndRefunds()
    {
        var student = CreateStudent("S001");
        var transactions = new[]
        {
            new PaymentTransaction(student, 500m, new DateTime(2026, 6, 1)),
            new PaymentTransaction(student, 600m, new DateTime(2026, 6, 2)),
            new PaymentTransaction(student, -100m, new DateTime(2026, 6, 3)),
        };

        var service = new FinancialReportingService();

        var total = service.GetTotalPaid(
            student,
            transactions,
            new DateTime(2026, 6, 30));

        Assert.Equal(1000m, total);
    }

    /// <summary>
    /// Verifies that transactions after the reporting date are ignored.
    /// </summary>
    [Fact]
    public void GetTotalPaid_ShouldIgnoreTransactionsAfterReportingDate()
    {
        var student = CreateStudent("S001");
        var transactions = new[]
        {
            new PaymentTransaction(student, 500m, new DateTime(2026, 6, 1)),
            new PaymentTransaction(student, 300m, new DateTime(2026, 7, 1)),
        };

        var service = new FinancialReportingService();

        var total = service.GetTotalPaid(
            student,
            transactions,
            new DateTime(2026, 6, 30));

        Assert.Equal(500m, total);
    }

    /// <summary>
    /// Verifies that the average paid amount per student is calculated.
    /// </summary>
    [Fact]
    public void GetAveragePaidPerStudent_ShouldCalculateNetAverage()
    {
        var studentA = CreateStudent("S001");
        var studentB = CreateStudent("S002");

        var transactions = new[]
        {
            new PaymentTransaction(studentA, 500m, new DateTime(2026, 6, 1)),
            new PaymentTransaction(studentA, -100m, new DateTime(2026, 6, 2)),
            new PaymentTransaction(studentB, 800m, new DateTime(2026, 6, 1)),
        };

        var service = new FinancialReportingService();

        var average = service.GetAveragePaidPerStudent(
            new[] { studentA, studentB },
            transactions,
            new DateTime(2026, 6, 30));

        Assert.Equal(600m, average);
    }

    private static Student CreateStudent(string registrationNumber)
    {
        return new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            registrationNumber,
            new[] { "0722123456" },
            Array.Empty<string>());
    }
}
