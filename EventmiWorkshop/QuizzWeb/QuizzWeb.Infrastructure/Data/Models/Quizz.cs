﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace QuizzWeb.Infrastructure.Data.Models
{
    /// <summary>
    /// Куиз
    /// </summary>
    [Comment("Куиз")]
    public class Quizz
    {
        public Quizz()
        {
            this.Questions = new HashSet<Question>();
        }

        /// <summary>
        /// Идентификатор на куиз
        /// </summary>
        [Comment("Идентификатор на куиз")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Име на куиз
        /// </summary>
        [Comment("Име на куиз")]
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Колекция с въпроси
        /// </summary>
        [Comment("Колекция с въпроси")]
        public virtual ICollection<Question> Questions { get; set; } = null!;
    }
}
