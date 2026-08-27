namespace UniversityManagement.Domain.Entities;

using System.Text.RegularExpressions;

public class Student
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Address { get; }
    public string Cnp { get; }
    public string RegistrationNumber { get; }
    private readonly List<string> _phoneNumbers;
    private readonly List<string> _emails;

    public IReadOnlyCollection<string> PhoneNumbers => _phoneNumbers;
    public IReadOnlyCollection<string> Emails => _emails;

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
            throw new ArgumentException("First name is required.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required.", nameof(address));

        if (string.IsNullOrWhiteSpace(cnp))
            throw new ArgumentException("CNP is required.", nameof(cnp));

        if (!Regex.IsMatch(cnp, @"^\d{13}$"))
            throw new ArgumentException(
                "CNP must contain exactly 13 digits.",
                nameof(cnp));

        if (string.IsNullOrWhiteSpace(registrationNumber))
            throw new ArgumentException(
                "Registration number is required.",
                nameof(registrationNumber));

        ArgumentNullException.ThrowIfNull(phoneNumbers);
        ArgumentNullException.ThrowIfNull(emails);

        _phoneNumbers = phoneNumbers.ToList();
        _emails = emails.ToList();

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

        if (_phoneNumbers.Any(phoneNumber =>
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

        if (_emails.Any(email =>
                string.IsNullOrWhiteSpace(email) ||
                !Regex.IsMatch(email, emailPattern)))
        {
            throw new ArgumentException(
                "Invalid email address.",
                nameof(emails));
        }

        if (_phoneNumbers.Count == 0 && _emails.Count == 0)
            throw new ArgumentException(
                "At least one phone number or email is required.");

        FirstName = firstName;
        LastName = lastName;
        Address = address;
        Cnp = cnp;
        RegistrationNumber = registrationNumber;
    }
}