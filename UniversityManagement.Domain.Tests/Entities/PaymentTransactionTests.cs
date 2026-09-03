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

    /// <summary>
    /// Verifies that a new payment transaction starts without a persistent identifier.
    /// </summary>
    [Fact]
    public void PaymentTransaction_ShouldStartWithZeroId()
    {
        var student = new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "12345",
            new[] { "0722123456" },
            Array.Empty<string>());

        var transaction = new PaymentTransaction(
            student,
            500m,
            new DateTime(2026, 6, 10));

        Assert.Equal(0, transaction.Id);
    }

    /// <summary>
    /// Verifies that an existing payment transaction can store a persistent identifier.
    /// </summary>
    [Fact]
    public void PaymentTransaction_ShouldStorePositiveId()
    {
        var student = new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "12345",
            new[] { "0722123456" },
            Array.Empty<string>());

        var transaction = new PaymentTransaction(
            1,
            student,
            500m,
            new DateTime(2026, 6, 10));

        Assert.Equal(1, transaction.Id);
    }

    /// <summary>
    /// Verifies that an existing payment transaction rejects a non-positive identifier.
    /// </summary>
    /// <param name="id">The invalid identifier to test.</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void PaymentTransaction_ShouldRejectNonPositiveId(int id)
    {
        var student = new Student(
            "Ion",
            "Popescu",
            "Brasov",
            "1234567890123",
            "12345",
            new[] { "0722123456" },
            Array.Empty<string>());

        Assert.Throws<ArgumentException>(
            () => new PaymentTransaction(
                id,
                student,
                500m,
                new DateTime(2026, 6, 10)));
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
