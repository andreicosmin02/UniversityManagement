// <copyright file="CourseRegistrationService.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services;

using System.Collections.Generic;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Orchestrates course selection, discount calculation, and payment creation.
/// </summary>
public class CourseRegistrationService
{
    private readonly CourseSelectionService courseSelectionService = new ();
    private readonly CourseCostService courseCostService = new ();
    private readonly CourseCurrencyService courseCurrencyService = new ();

    /// <summary>
    /// Registers selected courses and creates the corresponding payment transaction.
    /// </summary>
    /// <param name="student">The student registering for the courses.</param>
    /// <param name="semester">The semester in which the courses are selected.</param>
    /// <param name="selectedCourses">The courses selected in this operation.</param>
    /// <param name="existingEnrollments">The enrollments that already exist.</param>
    /// <param name="examAttempts">The exam history used to validate prerequisites.</param>
    /// <param name="discountRule">The discount combination relevant to the selection.</param>
    /// <param name="registrationDate">The selection and payment date.</param>
    /// <returns>The created enrollments and their payment transaction.</returns>
    public (IReadOnlyCollection<Enrollment> Enrollments, PaymentTransaction Payment)
        RegisterCourses(
            Student student,
            Semester semester,
            IEnumerable<Course> selectedCourses,
            IEnumerable<Enrollment> existingEnrollments,
            IEnumerable<ExamAttempt> examAttempts,
            DiscountRule discountRule,
            DateTime registrationDate)
    {
        ArgumentNullException.ThrowIfNull(student);
        ArgumentNullException.ThrowIfNull(semester);
        ArgumentNullException.ThrowIfNull(selectedCourses);
        ArgumentNullException.ThrowIfNull(existingEnrollments);
        ArgumentNullException.ThrowIfNull(examAttempts);
        ArgumentNullException.ThrowIfNull(discountRule);

        var selectedCourseList = selectedCourses.ToList();

        if (selectedCourseList.Count == 0)
        {
            throw new ArgumentException(
                "At least one course must be selected.",
                nameof(selectedCourses));
        }

        var enrollmentList = existingEnrollments.ToList();
        var attemptList = examAttempts.ToList();
        var createdEnrollments = new List<Enrollment>();

        this.courseCurrencyService.ValidateSameCurrency(selectedCourseList);

        foreach (var course in selectedCourseList)
        {
            var enrollment = this.courseSelectionService.SelectCourse(
                student,
                course,
                semester,
                enrollmentList,
                attemptList);

            createdEnrollments.Add(enrollment);
            enrollmentList.Add(enrollment);
        }

        this.courseSelectionService.ValidateMinimumSelectedCredits(
            student,
            semester,
            enrollmentList);

        var totalCost = this.courseCostService.CalculateTotal(
            selectedCourseList,
            discountRule);
        var payment = new PaymentTransaction(
            student,
            totalCost,
            registrationDate);

        return (createdEnrollments.AsReadOnly(), payment);
    }
}
