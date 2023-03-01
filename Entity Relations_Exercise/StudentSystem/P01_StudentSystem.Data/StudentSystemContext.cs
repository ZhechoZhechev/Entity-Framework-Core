namespace P01_StudentSystem.Data;

using Microsoft.EntityFrameworkCore;

using Common;
using Models;

public class StudentSystemContext : DbContext
{
    public StudentSystemContext()
    {
        
    }

    public StudentSystemContext(DbContextOptions options)
        :base(options)
    {
        
    }

    public DbSet<Student> Students { get; set; }

    public DbSet<Course> Courses { get; set; }

    public DbSet<Resource> Resources { get; set; }

    public DbSet<Homework> Homeworks { get; set; }

    public DbSet<StudentCourse> StudentsCourses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) 
        {
            optionsBuilder.UseSqlServer(DbConfig.ConnectionString);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>(en =>
        {
            en.Property(p => p.PhoneNumber)
            .IsFixedLength()
            .IsUnicode(false);
        });

        modelBuilder.Entity<Resource>(en =>
        {
            en.Property(p => p.Url)
            .IsUnicode(false);
        });

        modelBuilder.Entity<Homework>(en =>
        {
            en.Property(p => p.Content)
            .IsUnicode(false);
        });

        modelBuilder.Entity<StudentCourse>(en =>
        {
            en.HasKey(pk => new {pk.StudentId, pk.CourseId });
        });
    }
}