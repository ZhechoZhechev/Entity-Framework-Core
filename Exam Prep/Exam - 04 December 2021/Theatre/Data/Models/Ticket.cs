﻿namespace Theatre.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Reflection.Metadata.Ecma335;

    public class Ticket
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public sbyte RowNumber  { get; set; }

        [Required]
        [ForeignKey(nameof(Play))]
        public int PlayId  { get; set; }
        public Play Play { get; set; }

        [Required]
        [ForeignKey(nameof(Theatre))]
        public int TheatreId  { get; set; }
        public Theatre Theatre { get; set; }
    }
}