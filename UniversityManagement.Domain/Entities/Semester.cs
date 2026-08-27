namespace UniversityManagement.Domain.Entities;

public class Semester
{
    private readonly List<Course> _courses = new();

    public int Number { get; }
    public int MinimumCredits { get; }
    public IReadOnlyCollection<Course> Courses => _courses;
    public int TotalAvailableCredits =>
        _courses.Sum(course => course.Credits);
    public bool HasEnoughAvailableCredits =>
        TotalAvailableCredits >= MinimumCredits;

    public Semester(int number, int minimumCredits)
    {
        if (number <= 0)
            throw new ArgumentException("Semester number must be greater than zero.");

        if (minimumCredits < 0)
            throw new ArgumentException("Minimum credits cannot be negative.");

        Number = number;
        MinimumCredits = minimumCredits;
    }

    public void AddCourse(Course course)
    {
        if (_courses.Contains(course))
            throw new ArgumentException("Course is already added to this semester.");

        _courses.Add(course);
    }
}