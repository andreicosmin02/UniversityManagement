// <copyright file="PaymentTransactionRepositoryTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Tests.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Data.Repositories;
using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests persistence operations for payment transactions.
/// </summary>
public class PaymentTransactionRepositoryTests
{
    /// <summary>
    /// Verifies that adding a payment transaction persists it.
    /// </summary>
    [Fact]
    public void Add_ShouldPersistPaymentTransaction()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

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

            var repository = new PaymentTransactionRepository(context);

            repository.Add(transaction);

            Assert.True(transaction.Id > 0);
            Assert.Equal(1, context.PaymentTransactions.Count());
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that a payment transaction and its student can be retrieved.
    /// </summary>
    [Fact]
    public void GetById_ShouldLoadStudent()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

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

            var repository = new PaymentTransactionRepository(context);
            repository.Add(transaction);

            context.ChangeTracker.Clear();

            var storedTransaction = repository.GetById(transaction.Id);

            Assert.NotNull(storedTransaction);
            Assert.Equal(500m, storedTransaction.Amount);
            Assert.Equal(
                new DateTime(2026, 6, 10),
                storedTransaction.TransactionDate);
            Assert.Equal("12345", storedTransaction.Student.RegistrationNumber);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }

    /// <summary>
    /// Verifies that an unknown identifier returns no payment transaction.
    /// </summary>
    [Fact]
    public void GetById_ShouldReturnNullForUnknownId()
    {
        var databaseName = $"UniversityManagementTests_{Guid.NewGuid():N}";
        var options = new DbContextOptionsBuilder<UniversityManagementDbContext>()
            .UseSqlServer(
                $"Server=localhost;Database={databaseName};Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new UniversityManagementDbContext(options);

        try
        {
            context.Database.EnsureCreated();

            var repository = new PaymentTransactionRepository(context);

            var transaction = repository.GetById(999);

            Assert.Null(transaction);
        }
        finally
        {
            context.Database.EnsureDeleted();
        }
    }
}
