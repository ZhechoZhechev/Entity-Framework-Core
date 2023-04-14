using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizzWeb.Infrastructure.Data.Models
{

    /// <summary>
    /// Въпроси към даден куиз
    /// </summary>
    public class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }

        /// <summary>
        /// Идентификатор на въпрос
        /// </summary>
        [Comment("Идентификатор на въпрос")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Описание на въпрос
        /// </summary>
        [Comment("Съдържание на въпрос")]
        [Required]
        [StringLength(500)]
        public string QuestionText { get; set; } = null!;

        /// <summary>
        /// Към кой куиз е въпросът
        /// </summary>
        [Comment("Към кой куиз е въпросът")]
        [Required]
        [ForeignKey(nameof(Quizz))]
        public int QuizzId { get; set; }

        public virtual Quizz Quizz { get; set; } = null!;

        /// <summary>
        /// Отговори на съответният въпрос
        /// </summary>
        [Comment("Отговори на съответният въпрос")]
        public virtual ICollection<Answer> Answers { get; set; } = null!;
    }
}