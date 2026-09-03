// <copyright file="StudentTests.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Tests.Entities
{
    using UniversityManagement.Domain.Entities;
    using Xunit;

    /// <summary>
    /// Unit tests for the Student entity.
    /// </summary>
    public class StudentTests
    {
        /// <summary>
        /// Verifies that a student stores all required information.
        /// </summary>
        [Fact]
        public void Student_ShouldStoreRequiredInformation()
        {
            var student = new Student(
                "Ion",
                "Popescu",
                "Strada Exemplu 10",
                "1234567890123",
                "A001",
                Array.Empty<string>(),
                new[] { "ion@example.com" });

            Assert.Equal("Ion", student.FirstName);
            Assert.Equal("Popescu", student.LastName);
            Assert.Equal("Strada Exemplu 10", student.Address);
            Assert.Equal("1234567890123", student.Cnp);
            Assert.Equal("A001", student.RegistrationNumber);
        }

        /// <summary>
        /// Verifies that empty or whitespace first names are rejected.
        /// </summary>
        /// <param name="firstName">The first name value to test.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Student_ShouldRejectEmptyOrWhitespaceFirstName(string firstName)
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    firstName,
                    "Popescu",
                    "Strada Exemplu 10",
                    "1234567890123",
                    "A001",
                    Array.Empty<string>(),
                    new[] { "ion@example.com" }));
        }

        /// <summary>
        /// Verifies that empty or whitespace last names are rejected.
        /// </summary>
        /// <param name="lastName">The last name value to test.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Student_ShouldRejectEmptyOrWhitespaceLastName(string lastName)
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    "Ion",
                    lastName,
                    "Strada Exemplu 10",
                    "1234567890123",
                    "A001",
                    Array.Empty<string>(),
                    new[] { "ion@example.com" }));
        }

        /// <summary>
        /// Verifies that empty or whitespace addresses are rejected.
        /// </summary>
        /// <param name="address">The address value to test.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Student_ShouldRejectEmptyOrWhitespaceAddress(string address)
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    "Ion",
                    "Popescu",
                    address,
                    "1234567890123",
                    "A001",
                    Array.Empty<string>(),
                    new[] { "ion@example.com" }));
        }

        /// <summary>
        /// Verifies that empty or whitespace CNPs are rejected.
        /// </summary>
        /// <param name="cnp">The CNP value to test.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Student_ShouldRejectEmptyOrWhitespaceCnp(string cnp)
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    "Ion",
                    "Popescu",
                    "Strada Exemplu 10",
                    cnp,
                    "A001",
                    Array.Empty<string>(),
                    new[] { "ion@example.com" }));
        }

        /// <summary>
        /// Verifies that empty or whitespace registration numbers are rejected.
        /// </summary>
        /// <param name="registrationNumber">The registration number value to test.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Student_ShouldRejectEmptyOrWhitespaceRegistrationNumber(
            string registrationNumber)
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    "Ion",
                    "Popescu",
                    "Strada Exemplu 10",
                    "1234567890123",
                    registrationNumber,
                    Array.Empty<string>(),
                    new[] { "ion@example.com" }));
        }

        /// <summary>
        /// Verifies that a student stores phone numbers.
        /// </summary>
        [Fact]
        public void Student_ShouldStorePhoneNumbers()
        {
            var student = new Student(
                "Ion",
                "Popescu",
                "Strada Exemplu 10",
                "1234567890123",
                "A001",
                new[] { "0722 123456" },
                Array.Empty<string>());

            Assert.Contains("0722 123456", student.PhoneNumbers);
        }

        /// <summary>
        /// Verifies that a student stores email addresses.
        /// </summary>
        [Fact]
        public void Student_ShouldStoreEmails()
        {
            var student = new Student(
                "Ion",
                "Popescu",
                "Strada Exemplu 10",
                "1234567890123",
                "A001",
                Array.Empty<string>(),
                new[] { "ion@example.com" });

            Assert.Contains("ion@example.com", student.Emails);
        }

        /// <summary>
        /// Verifies that a student must have at least one contact method.
        /// </summary>
        [Fact]
        public void Student_ShouldRejectMissingPhoneNumbersAndEmails()
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    "Ion",
                    "Popescu",
                    "Strada Exemplu 10",
                    "1234567890123",
                    "A001",
                    Array.Empty<string>(),
                    Array.Empty<string>()));
        }

        /// <summary>
        /// Verifies accepted Romanian phone number formats.
        /// </summary>
        /// <param name="phoneNumber">The phone number value to test.</param>
        [Theory]
        [InlineData("0722123456")]
        [InlineData("0722 123456")]
        public void Student_ShouldAcceptValidPhoneNumberFormats(string phoneNumber)
        {
            var student = new Student(
                "Ion",
                "Popescu",
                "Strada Exemplu 10",
                "1234567890123",
                "A001",
                new[] { phoneNumber },
                Array.Empty<string>());

            Assert.Contains(phoneNumber, student.PhoneNumbers);
        }

        /// <summary>
        /// Verifies rejected invalid phone number formats.
        /// </summary>
        /// <param name="phoneNumber">The phone number value to test.</param>
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("abc")]
        [InlineData("0722ABC456")]
        [InlineData("0722 123 456")]
        [InlineData("0722-123-456")]
        [InlineData("+40 722 123 456")]
        [InlineData("+1 (415) 555-2671")]
        [InlineData("123")]
        [InlineData("1234567890123456")]
        public void Student_ShouldRejectInvalidPhoneNumberFormats(string phoneNumber)
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    "Ion",
                    "Popescu",
                    "Strada Exemplu 10",
                    "1234567890123",
                    "A001",
                    new[] { phoneNumber },
                    Array.Empty<string>()));
        }

        /// <summary>
        /// Verifies accepted email address formats.
        /// </summary>
        /// <param name="email">The email address value to test.</param>
        [Theory]
        [InlineData("ion@example.com")]
        [InlineData("ion.popescu@example.com")]
        [InlineData("ion+student@example.com")]
        [InlineData("student_123@example.com")]
        [InlineData("student@example")]
        [InlineData("student@sub.example.co.uk")]
        [InlineData("john-doe@example-domain.com")]
        public void Student_ShouldAcceptValidEmailFormats(string email)
        {
            var student = new Student(
                "Ion",
                "Popescu",
                "Strada Exemplu 10",
                "1234567890123",
                "A001",
                Array.Empty<string>(),
                new[] { email });

            Assert.Contains(email, student.Emails);
        }

        /// <summary>
        /// Verifies rejected invalid email formats.
        /// </summary>
        /// <param name="email">The email address value to test.</param>
        [Theory]
        [InlineData("")]
        [InlineData("ionexample.com")]
        [InlineData("@example.com")]
        [InlineData("ion@")]
        [InlineData("ion@@example.com")]
        [InlineData("ion popescu@example.com")]
        [InlineData(".ion@example.com")]
        [InlineData("ion.@example.com")]
        [InlineData("ion..popescu@example.com")]
        [InlineData("ion@-example.com")]
        [InlineData("ion@example-.com")]
        [InlineData("ion@example..com")]
        public void Student_ShouldRejectInvalidEmailFormats(string email)
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    "Ion",
                    "Popescu",
                    "Strada Exemplu 10",
                    "1234567890123",
                    "A001",
                    Array.Empty<string>(),
                    new[] { email }));
        }

        /// <summary>
        /// Verifies rejected invalid CNP formats.
        /// </summary>
        /// <param name="cnp">The CNP value to test.</param>
        [Theory]
        [InlineData("123456789012")]
        [InlineData("12345678901234")]
        [InlineData("123456789012A")]
        [InlineData("1234 567890123")]
        public void Student_ShouldRejectInvalidCnpFormat(string cnp)
        {
            Assert.Throws<ArgumentException>(() =>
                new Student(
                    "Ion",
                    "Popescu",
                    "Strada Exemplu 10",
                    cnp,
                    "A001",
                    Array.Empty<string>(),
                    new[] { "ion@example.com" }));
        }

        /// <summary>
        /// Verifies that a new student starts without a persistent identifier.
        /// </summary>
        [Fact]
        public void Student_ShouldStartWithZeroId()
        {
            var student = new Student(
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                ["0722123456"],
                []);

            Assert.Equal(0, student.Id);
        }

        /// <summary>
        /// Verifies that an existing student can store a persistent identifier.
        /// </summary>
        [Fact]
        public void Student_ShouldStorePositiveId()
        {
            var student = new Student(
                1,
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                ["0722123456"],
                []);

            Assert.Equal(1, student.Id);
        }

        /// <summary>
        /// Verifies that an existing student cannot have a non-positive identifier.
        /// </summary>
        /// <param name="id">The invalid identifier to test.</param>
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Student_ShouldRejectNonPositiveId(int id)
        {
            Assert.Throws<ArgumentException>(() => new Student(
                id,
                "Ion",
                "Popescu",
                "Brasov",
                "1234567890123",
                "12345",
                ["0722123456"],
                []));
        }
    }
}
