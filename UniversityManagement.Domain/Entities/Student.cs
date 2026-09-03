// <copyright file="Student.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Domain.Entities;

using System.Text.RegularExpressions;

/// <summary>
/// Represents a student and their contact information.
/// </summary>
public class Student
{
    private readonly List<string> phoneNumbers;
    private readonly List<string> emails;

    /// <summary>
    /// Initializes a new instance of the <see cref="Student"/> class.
    /// </summary>
    /// <param name="firstName">The student's first name.</param>
    /// <param name="lastName">The student's last name.</param>
    /// <param name="address">The student's address.</param>
    /// <param name="cnp">The student's 13-digit personal numeric code.</param>
    /// <param name="registrationNumber">The student's registration number.</param>
    /// <param name="phoneNumbers">The student's phone numbers.</param>
    /// <param name="emails">The student's email addresses.</param>
    public Student(
        string firstName,
        string lastName,
        string address,
        string cnp,
        string registrationNumber,
        IEnumerable<string> phoneNumbers,
        IEnumerable<string> emails)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name is required.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name is required.", nameof(lastName));
        }

        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ArgumentException("Address is required.", nameof(address));
        }

        if (string.IsNullOrWhiteSpace(cnp))
        {
            throw new ArgumentException("CNP is required.", nameof(cnp));
        }

        if (!Regex.IsMatch(cnp, @"^\d{13}$"))
        {
            throw new ArgumentException(
                "CNP must contain exactly 13 digits.",
                nameof(cnp));
        }

        if (string.IsNullOrWhiteSpace(registrationNumber))
        {
            throw new ArgumentException(
                "Registration number is required.",
                nameof(registrationNumber));
        }

        ArgumentNullException.ThrowIfNull(phoneNumbers);
        ArgumentNullException.ThrowIfNull(emails);

        this.phoneNumbers = phoneNumbers.ToList();
        this.emails = emails.ToList();

        // Accepts common local and international phone formats:
        // - 7 to 15 digits in total
        // - optional international prefix: + followed by 1-3 digits
        // - separators: space, hyphen or dot
        // - optional parentheses around a group, e.g. +1 (415) 555-2671
        // Examples: 0722123456, 0722 123 456, 0722-123-456,
        //           +40 722 123 456, +44 20 7946 0958, +1 (415) 555-2671
        // This validates the format only; it does not verify that the country code
        // or the phone number actually exists.
        const string phonePattern =
            @"^(?=(?:\D*\d){7,15}\D*$)(?:\+\d{1,3}[ .-]?)?(?:\(\d{1,4}\)|\d{1,4})(?:[ .-]?(?:\(\d{1,4}\)|\d{1,4})){1,4}$";

        if (this.phoneNumbers.Any(phoneNumber =>
            string.IsNullOrWhiteSpace(phoneNumber) ||
            !Regex.IsMatch(phoneNumber, phonePattern)))
        {
            throw new ArgumentException(
                "Invalid phone number.",
                nameof(phoneNumbers));
        }

        // Accepts common email formats:
        // - letters, digits and common symbols in the local part
        // - dots inside the local part, but not at the beginning/end or consecutively
        // - domains with one or more labels
        // - hyphens inside domain labels, but not at their beginning/end
        // Examples: ion@example.com, ion.popescu@example.com,
        //           ion+student@example.com, student@sub.example.co.uk,
        //           student@example
        // This validates the syntax only; it does not verify that the domain
        // or mailbox actually exists.
        const string emailPattern =
            @"^(?=.{1,254}$)(?=.{1,64}@)" +
            @"[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+" +
            @"(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*" +
            @"@" +
            @"[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?" +
            @"(?:\.[A-Za-z0-9](?:[A-Za-z0-9-]{0,61}[A-Za-z0-9])?)*$";

        if (this.emails.Any(email =>
                string.IsNullOrWhiteSpace(email) ||
                !Regex.IsMatch(email, emailPattern)))
        {
            throw new ArgumentException(
                "Invalid email address.",
                nameof(emails));
        }

        if (this.phoneNumbers.Count == 0 && this.emails.Count == 0)
        {
            throw new ArgumentException(
                "At least one phone number or email is required.");
        }

        this.FirstName = firstName;
        this.LastName = lastName;
        this.Address = address;
        this.Cnp = cnp;
        this.RegistrationNumber = registrationNumber;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Student"/> class
    /// with an existing persistent identifier.
    /// </summary>
    /// <param name="id">The persistent identifier.</param>
    /// <param name="firstName">The student's first name.</param>
    /// <param name="lastName">The student's last name.</param>
    /// <param name="address">The student's address.</param>
    /// <param name="cnp">The student's CNP.</param>
    /// <param name="registrationNumber">The student's registration number.</param>
    /// <param name="phoneNumbers">The student's phone numbers.</param>
    /// <param name="emails">The student's email addresses.</param>
    public Student(
        int id,
        string firstName,
        string lastName,
        string address,
        string cnp,
        string registrationNumber,
        IEnumerable<string> phoneNumbers,
        IEnumerable<string> emails)
        : this(
            firstName,
            lastName,
            address,
            cnp,
            registrationNumber,
            phoneNumbers,
            emails)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Student identifier must be greater than zero.",
                nameof(id));
        }

        this.Id = id;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Student"/> class for persistence.
    /// </summary>
    /// <param name="firstName">The student's first name.</param>
    /// <param name="lastName">The student's last name.</param>
    /// <param name="address">The student's address.</param>
    /// <param name="cnp">The student's CNP.</param>
    /// <param name="registrationNumber">The student's registration number.</param>
    private Student(
        string firstName,
        string lastName,
        string address,
        string cnp,
        string registrationNumber)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Address = address;
        this.Cnp = cnp;
        this.RegistrationNumber = registrationNumber;
        this.phoneNumbers = new List<string>();
        this.emails = new List<string>();
    }

    /// <summary>
    /// Gets the persistent identifier of the student.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// Gets the student's first name.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the student's last name.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Gets the student's address.
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Gets the student's personal numeric code.
    /// </summary>
    public string Cnp { get; }

    /// <summary>
    /// Gets the student's registration number.
    /// </summary>
    public string RegistrationNumber { get; }

    /// <summary>
    /// Gets the student's phone numbers.
    /// </summary>
    public IReadOnlyCollection<string> PhoneNumbers => this.phoneNumbers;

    /// <summary>
    /// Gets the student's email addresses.
    /// </summary>
    public IReadOnlyCollection<string> Emails => this.emails;
}
