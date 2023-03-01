namespace P01_StudentSystem.Data.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;
public class Homework
{
    [Key]
    public int HomeworkId { get; set; }

    [Required]
    public string Content { get; set; } = null!;

    [Required]
    public ContentType ContentType { get; set; }

    [Required]
    public DateTime SubmissionTime { get; set; }

    [Required]
    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }
    public virtual Student Student { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
}
