// <copyright file="CourseRegistrationServiceTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Services.Tests;

using UniversityManagement.Domain.Entities;
using Xunit;

/// <summary>
/// Tests course registration and payment orchestration.
/// </summary>
public class CourseRegistrationServiceTests
{
    /// <summary>
    /// Verifies that registration creates one enrollment for each selected course.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldCreateEnrollmentsForSelectedCourses()
    {
        var student = CreateStudent();
        var courseA = CreateCourse("A", 5, 500m);
        var courseB = CreateCourse("B", 5, 600m);
        var semester = CreateSemester(10, courseA, courseB);
        var discountRule = new DiscountRule(new[] { CreateCourse("C", 5, 700m) }, 10m);
        var service = new CourseRegistrationService();

        var result = service.RegisterCourses(
            student,
            semester,
            new[] { courseA, courseB },
            Array.Empty<Enrollment>(),
            Array.Empty<ExamAttempt>(),
            discountRule,
            new DateTime(2026, 9, 1));

        Assert.Equal(2, result.Enrollments.Count);
        Assert.Contains(result.Enrollments, enrollment => ReferenceEquals(enrollment.Course, courseA));
        Assert.Contains(result.Enrollments, enrollment => ReferenceEquals(enrollment.Course, courseB));
    }

    /// <summary>
    /// Verifies that a selection without an applicable discount is paid at full price.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldCreatePaymentForFullCourseCost()
    {
        var student = CreateStudent();
        var courseA = CreateCourse("A", 5, 500m);
        var courseB = CreateCourse("B", 5, 600m);
        var semester = CreateSemester(10, courseA, courseB);
        var discountRule = new DiscountRule(new[] { CreateCourse("C", 5, 700m) }, 10m);
        var service = new CourseRegistrationService();

        var result = service.RegisterCourses(
            student,
            semester,
            new[] { courseA, courseB },
            Array.Empty<Enrollment>(),
            Array.Empty<ExamAttempt>(),
            discountRule,
            new DateTime(2026, 9, 1));

        Assert.Equal(1100m, result.Payment.Amount);
    }

    /// <summary>
    /// Verifies that a complete course combination receives its discount.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldApplyCompleteCombinationDiscount()
    {
        var student = CreateStudent();
        var courseA = CreateCourse("A", 5, 500m);
        var courseB = CreateCourse("B", 5, 600m);
        var semester = CreateSemester(10, courseA, courseB);
        var discountRule = new DiscountRule(new[] { courseA, courseB }, 10m);
        var service = new CourseRegistrationService();

        var result = service.RegisterCourses(
            student,
            semester,
            new[] { courseA, courseB },
            Array.Empty<Enrollment>(),
            Array.Empty<ExamAttempt>(),
            discountRule,
            new DateTime(2026, 9, 1));

        Assert.Equal(990m, result.Payment.Amount);
    }

    /// <summary>
    /// Verifies that an incomplete discount combination leaves selected courses at full price.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldUseFullPriceForIncompleteDiscountCombination()
    {
        var student = CreateStudent();
        var courseA = CreateCourse("A", 5, 500m);
        var courseB = CreateCourse("B", 5, 600m);
        var courseC = CreateCourse("C", 5, 700m);
        var semester = CreateSemester(10, courseA, courseB);
        var discountRule = new DiscountRule(new[] { courseA, courseC }, 10m);
        var service = new CourseRegistrationService();

        var result = service.RegisterCourses(
            student,
            semester,
            new[] { courseA, courseB },
            Array.Empty<Enrollment>(),
            Array.Empty<ExamAttempt>(),
            discountRule,
            new DateTime(2026, 9, 1));

        Assert.Equal(1100m, result.Payment.Amount);
    }

    /// <summary>
    /// Verifies that a payment stores the registering student and operation date.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldStoreStudentAndPaymentDate()
    {
        var student = CreateStudent();
        var course = CreateCourse("A", 5, 500m);
        var semester = CreateSemester(5, course);
        var discountRule = new DiscountRule(new[] { CreateCourse("B", 5, 500m) }, 10m);
        var paymentDate = new DateTime(2026, 9, 1, 14, 30, 0);
        var service = new CourseRegistrationService();

        var result = service.RegisterCourses(
            student,
            semester,
            new[] { course },
            Array.Empty<Enrollment>(),
            Array.Empty<ExamAttempt>(),
            discountRule,
            paymentDate);

        Assert.Same(student, result.Payment.Student);
        Assert.Equal(paymentDate, result.Payment.TransactionDate);
    }

    /// <summary>
    /// Verifies that an empty course selection is rejected.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldRejectEmptySelection()
    {
        var student = CreateStudent();
        var semester = new Semester(1, 0);
        var discountRule = new DiscountRule(new[] { CreateCourse("A", 5, 500m) }, 10m);
        var service = new CourseRegistrationService();

        Assert.Throws<ArgumentException>(
            () => service.RegisterCourses(
                student,
                semester,
                Array.Empty<Course>(),
                Array.Empty<Enrollment>(),
                Array.Empty<ExamAttempt>(),
                discountRule,
                new DateTime(2026, 9, 1)));
    }

    /// <summary>
    /// Verifies that courses using different currencies cannot be registered together.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldRejectMixedCurrencies()
    {
        var student = CreateStudent();
        var courseA = CreateCourse("A", 5, 500m, "RON");
        var courseB = CreateCourse("B", 5, 600m, "EUR");
        var semester = CreateSemester(10, courseA, courseB);
        var discountRule = new DiscountRule(new[] { courseA, courseB }, 10m);
        var service = new CourseRegistrationService();

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterCourses(
                student,
                semester,
                new[] { courseA, courseB },
                Array.Empty<Enrollment>(),
                Array.Empty<ExamAttempt>(),
                discountRule,
                new DateTime(2026, 9, 1)));
    }

    /// <summary>
    /// Verifies that a selection below the semester threshold is rejected.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldRejectSelectionBelowMinimumCredits()
    {
        var student = CreateStudent();
        var course = CreateCourse("A", 5, 500m);
        var semester = CreateSemester(10, course);
        var discountRule = new DiscountRule(new[] { course }, 10m);
        var service = new CourseRegistrationService();

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterCourses(
                student,
                semester,
                new[] { course },
                Array.Empty<Enrollment>(),
                Array.Empty<ExamAttempt>(),
                discountRule,
                new DateTime(2026, 9, 1)));
    }

    /// <summary>
    /// Verifies that a course outside the semester cannot be registered.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldRejectUnavailableCourse()
    {
        var student = CreateStudent();
        var course = CreateCourse("A", 5, 500m);
        var semester = new Semester(1, 0);
        var discountRule = new DiscountRule(new[] { course }, 10m);
        var service = new CourseRegistrationService();

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterCourses(
                student,
                semester,
                new[] { course },
                Array.Empty<Enrollment>(),
                Array.Empty<ExamAttempt>(),
                discountRule,
                new DateTime(2026, 9, 1)));
    }

    /// <summary>
    /// Verifies that an unsatisfied prerequisite prevents registration.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldRejectUnsatisfiedPrerequisite()
    {
        var student = CreateStudent();
        var requiredCourse = CreateCourse("A", 5, 500m);
        var course = CreateCourse("B", 5, 600m);
        course.AddPrerequisite(new Prerequisite(requiredCourse, 5));
        var semester = CreateSemester(5, course);
        var discountRule = new DiscountRule(new[] { course }, 10m);
        var service = new CourseRegistrationService();

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterCourses(
                student,
                semester,
                new[] { course },
                Array.Empty<Enrollment>(),
                Array.Empty<ExamAttempt>(),
                discountRule,
                new DateTime(2026, 9, 1)));
    }

    /// <summary>
    /// Verifies that duplicate courses in one registration are rejected.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldRejectDuplicateCourseInSelection()
    {
        var student = CreateStudent();
        var course = CreateCourse("A", 5, 500m);
        var semester = CreateSemester(5, course);
        var discountRule = new DiscountRule(new[] { course }, 10m);
        var service = new CourseRegistrationService();

        Assert.Throws<InvalidOperationException>(
            () => service.RegisterCourses(
                student,
                semester,
                new[] { course, course },
                Array.Empty<Enrollment>(),
                Array.Empty<ExamAttempt>(),
                discountRule,
                new DateTime(2026, 9, 1)));
    }

    /// <summary>
    /// Verifies that the generated payment can be consumed by financial reporting.
    /// </summary>
    [Fact]
    public void RegisterCourses_ShouldCreateReportablePayment()
    {
        var student = CreateStudent();
        var course = CreateCourse("A", 5, 500m);
        var semester = CreateSemester(5, course);
        var discountRule = new DiscountRule(new[] { CreateCourse("B", 5, 500m) }, 10m);
        var paymentDate = new DateTime(2026, 9, 1);
        var service = new CourseRegistrationService();

        var result = service.RegisterCourses(
            student,
            semester,
            new[] { course },
            Array.Empty<Enrollment>(),
            Array.Empty<ExamAttempt>(),
            discountRule,
            paymentDate);
        var total = new FinancialReportingService().GetTotalPaid(
            student,
            new[] { result.Payment },
            paymentDate);

        Assert.Equal(500m, total);
    }

    private static Course CreateCourse(
        string name,
        int credits,
        decimal cost,
        string currency = "RON")
    {
        return new Course(
            name,
            $"Course {name}",
            credits,
            100m,
            cost,
            currency);
    }

    private static Semester CreateSemester(
        int minimumCredits,
        params Course[] courses)
    {
        var semester = new Semester(1, minimumCredits);

        foreach (var course in courses)
        {
            semester.AddCourse(course);
        }

        return semester;
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
