// <copyright file="FinancialReportingService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using System.Collections.Generic;
using System.Linq;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Provides financial reporting operations.
/// </summary>
public class FinancialReportingService
{
    /// <summary>
    /// Gets the total net amount paid by a student up to a specified date.
    /// </summary>
    /// <param name="student">The student to evaluate.</param>
    /// <param name="transactions">The payment and refund transactions.</param>
    /// <param name="reportingDate">The reporting date.</param>
    /// <returns>The net amount paid by the student.</returns>
    public decimal GetTotalPaid(
        Student student,
        IEnumerable<PaymentTransaction> transactions,
        DateTime reportingDate)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(transactions);

        return transactions
            .Where(transaction =>
                ReferenceEquals(transaction.Student, student)
                && transaction.TransactionDate <= reportingDate)
            .Sum(transaction => transaction.Amount);
    }

    /// <summary>
    /// Gets the average net amount paid per student up to a specified date.
    /// </summary>
    /// <param name="students">The students to include in the calculation.</param>
    /// <param name="transactions">The payment and refund transactions.</param>
    /// <param name="reportingDate">The reporting date.</param>
    /// <returns>The average net amount paid per student.</returns>
    public decimal GetAveragePaidPerStudent(
        IEnumerable<Student> students,
        IEnumerable<PaymentTransaction> transactions,
        DateTime reportingDate)
    {
        ArgumentNullException.ThrowIfNull(students);
        ArgumentNullException.ThrowIfNull(transactions);

        return students.Average(
            student => this.GetTotalPaid(
                student,
                transactions,
                reportingDate));
    }
}
