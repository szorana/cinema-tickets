﻿using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Domain
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}