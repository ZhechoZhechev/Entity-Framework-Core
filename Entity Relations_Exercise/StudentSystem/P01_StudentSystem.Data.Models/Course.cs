namespace P01_StudentSystem.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Course
{
    public Course()
    {
        this.Resources = new HashSet<Resource>();
        this.Homeworks = new HashSet<Homework>();
        this.StudentsCourses = new HashSet<StudentCourse>();
    }

    [Key]
    public int CourseId { get; set; }

    [StringLength(80)]
    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    public decimal Price { get; set; }

    public ICollection<Resource> Resources { get; set; } = null!;

    public ICollection<Homework> Homeworks { get; set; } = null!;

    public ICollection<StudentCourse> StudentsCourses { get; set; } = null!;
}
