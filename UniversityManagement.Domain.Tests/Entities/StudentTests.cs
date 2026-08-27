using UniversityManagement.Domain.Entities;
using Xunit;

namespace UniversityManagement.Domain.Tests.Entities;

public class StudentTests
{
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

    [Theory]
    [InlineData("0722123456")]
    [InlineData("0722 123 456")]
    [InlineData("0722-123-456")]
    [InlineData("0722.123.456")]
    [InlineData("+40 722 123 456")]
    [InlineData("+40-722-123-456")]
    [InlineData("+1 (415) 555-2671")]
    [InlineData("+44 20 7946 0958")]
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

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc")]
    [InlineData("0722ABC456")]
    [InlineData("++40 722 123 456")]
    [InlineData("+40 (722 123 456")]
    [InlineData("+40 722) 123456")]
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
}