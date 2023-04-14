using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QuizzWeb.Infrastructure.Data.Models
{
    /// <summary>
    /// Отговор
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Идентификатор на отговор
        /// </summary>
        [Comment("Идентификатор на отговор")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Съдържание на отговор
        /// </summary>
        [Comment("Съдържание на отговор")]
        [Required]
        [StringLength(500)]
        public string AnswerText { get; set; } = null!;

        /// <summary>
        /// Верен ли е отговорът
        /// </summary>
        [Comment("Верен ли е отговорът")]
        [Required]
        public bool IsCorrect { get; set; }

        /// <summary>
        /// Към кой въпрос се отнася отговорът
        /// </summary>
        [Comment("Към кой въпрос се отнася отговорът")]
        [Required]
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; } = null!;

    }
}