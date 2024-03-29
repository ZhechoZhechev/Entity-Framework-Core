﻿namespace VaporStore.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;

    public class ImportUsersCardsDto
    {
        [Required]
        [RegularExpression(@"^[A-Z][a-z]+ [A-Z][a-z]+$")]
        public string FullName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Range(3, 103)]
        public int Age { get; set; }

        public CardDto[] Cards { get; set; }
    }

    public class CardDto 
    {
        [Required]
        [RegularExpression(@"^\d{4} \d{4} \d{4} \d{4}$")]
        public string Number { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3)]
        public string CVC { get; set; }

        [Required]
        public string Type { get; set; }

    }
}
