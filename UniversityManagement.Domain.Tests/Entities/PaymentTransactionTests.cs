// <copyright file="PaymentTransactionTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Tests.Entities;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests the <see cref="PaymentTransaction"/> entity.
/// </summary>
public class PaymentTransactionTests
{
    /// <summary>
    /// Verifies that payment transaction data is stored.
    /// </summary>
    [Fact]
    public void PaymentTransaction_ShouldStoreStudentAmountAndDate()
    {
        var student = CreateStudent();

        var transaction = new PaymentTransaction(
            student,
            500m,
            new DateTime(2026, 6, 1));

        Assert.Same(student, transaction.Student);
        Assert.Equal(500m, transaction.Amount);
        Assert.Equal(new DateTime(2026, 6, 1), transaction.TransactionDate);
    }

    /// <summary>
    /// Verifies that a negative amount can represent a refund.
    /// </summary>
    [Fact]
    public void PaymentTransaction_ShouldAcceptRefund()
    {
        var student = CreateStudent();

        var transaction = new PaymentTransaction(
            student,
            -100m,
            new DateTime(2026, 6, 2));

        Assert.Equal(-100m, transaction.Amount);
    }

    /// <summary>
    /// Verifies that a zero-value transaction is rejected.
    /// </summary>
    [Fact]
    public void PaymentTransaction_ShouldRejectZeroAmount()
    {
        var student = CreateStudent();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new PaymentTransaction(
                student,
                0m,
                new DateTime(2026, 6, 1)));
    }

    private static Student CreateStudent()
    {
        return new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "S001",
            new[] { "0722123456" },
            Array.Empty<string>());
    }
}
