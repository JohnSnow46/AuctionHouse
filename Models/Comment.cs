﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuctionHouse.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Content { get; set; }

        [Required]
        public string? IdentityUserId { get; set; }

        [ForeignKey("IdentityUserId")]
        public IdentityUser? User { get; set; }

        public int? ListingId { get; set; }

        [ForeignKey("ListingId")]
        public Listing? Listing { get; set; }
    }
}
