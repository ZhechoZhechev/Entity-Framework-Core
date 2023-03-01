using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_StudentSystem.Data.Models;

public class StudentCourse
{
    [Required]
    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }
    public Student Student { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public Course Course { get; set;} = null!;
}
