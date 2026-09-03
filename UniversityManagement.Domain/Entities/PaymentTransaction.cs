// <copyright file="PaymentTransaction.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

/// <summary>
/// Represents a payment or refund associated with a student.
/// </summary>
public class PaymentTransaction
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentTransaction"/> class.
    /// </summary>
    /// <param name="student">The student associated with the transaction.</param>
    /// <param name="amount">
    /// The transaction amount. Positive values represent payments and negative values represent refunds.
    /// </param>
    /// <param name="transactionDate">The date of the transaction.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the transaction amount is zero.
    /// </exception>
    public PaymentTransaction(
        Student student,
        decimal amount,
        DateTime transactionDate)
    {
        ArgumentNullException.ThrowIfNull(student);

        if (amount == 0m)
        {
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "The transaction amount cannot be zero.");
        }

        this.Student = student;
        this.Amount = amount;
        this.TransactionDate = transactionDate;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentTransaction"/> class
    /// with an existing persistent identifier.
    /// </summary>
    /// <param name="id">The persistent identifier.</param>
    /// <param name="student">The student associated with the transaction.</param>
    /// <param name="amount">The transaction amount.</param>
    /// <param name="transactionDate">The transaction date.</param>
    public PaymentTransaction(
        int id,
        Student student,
        decimal amount,
        DateTime transactionDate)
        : this(student, amount, transactionDate)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Payment transaction identifier must be greater than zero.",
                nameof(id));
        }

        this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PaymentTransaction"/> class for persistence.
    /// </summary>
    private PaymentTransaction()
    {
        this.Student = null!;
    }

    /// <summary>
    /// Gets the persistent identifier of the payment transaction.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the student associated with the transaction.
    /// </summary>
    public Student Student { get; private set; }

    /// <summary>
    /// Gets the transaction amount.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// Gets the transaction date.
    /// </summary>
    public DateTime TransactionDate { get; private set; }
}
