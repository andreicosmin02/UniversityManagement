// <copyright file="UniversityManagementDbContext.cs" company="Universitatea Transilvania">
// Copyright (c) Universitatea Transilvania. All rights reserved.
// </copyright>

namespace UniversityManagement.Data.Persistence;

using Microsoft.EntityFrameworkCore;
using UniversityManagement.Domain.Entities;

/// <summary>
/// Represents the Entity Framework database context for university management data.
/// </summary>
public class UniversityManagementDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UniversityManagementDbContext"/> class.
    /// </summary>
    /// <param name="options">The options used to configure the database context.</param>
    public UniversityManagementDbContext(
        DbContextOptions<UniversityManagementDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Gets the courses stored in the database.
    /// </summary>
    public DbSet<Course> Courses => this.Set<Course>();

    /// <summary>
    /// Gets the students stored in the database.
    /// </summary>
    public DbSet<Student> Students => this.Set<Student>();

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(course => course.Id);

            entity.Property(course => course.Name);
            entity.Property(course => course.Description);
            entity.Property(course => course.Credits);
            entity.Property(course => course.MinimumCostPerCredit);
            entity.Property(course => course.Cost);

            entity.Ignore(course => course.Prerequisites);
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(student => student.Id);

            entity.Property(student => student.Id)
                .ValueGeneratedOnAdd();

            entity.Property(student => student.FirstName)
                .IsRequired();

            entity.Property(student => student.LastName)
                .IsRequired();

            entity.Property(student => student.Address)
                .IsRequired();

            entity.Property(student => student.Cnp)
                .IsRequired();

            entity.Property(student => student.RegistrationNumber)
                .IsRequired();

            entity.PrimitiveCollection(student => student.PhoneNumbers)
                .HasField("phoneNumbers");

            entity.PrimitiveCollection(student => student.Emails)
                .HasField("emails");
        });
    }
}
