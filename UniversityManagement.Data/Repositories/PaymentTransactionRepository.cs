// <copyright file="PaymentTransactionRepository.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Repositories;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Data.Persistence;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides persistence operations for payment transactions.
/// </summary>
public class PaymentTransactionRepository
{
    private readonly UniversityManagementDbContext context;

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentTransactionRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public PaymentTransactionRepository(UniversityManagementDbContext context)
    {
        this.context = context;
    }

    /// <summary>
    /// Adds and persists a payment transaction.
    /// </summary>
    /// <param name="transaction">The payment transaction to persist.</param>
    public void Add(PaymentTransaction transaction)
    {
        this.context.PaymentTransactions.Add(transaction);
        this.context.SaveChanges();
    }

    /// <summary>
    /// Gets a payment transaction by its persistent identifier.
    /// </summary>
    /// <param name="id">The payment transaction identifier.</param>
    /// <returns>
    /// The matching transaction, or <see langword="null"/> if none exists.
    /// </returns>
    public PaymentTransaction? GetById(int id)
    {
        return this.context.PaymentTransactions
            .Include(transaction => transaction.Student)
            .SingleOrDefault(transaction => transaction.Id == id);
    }
}
