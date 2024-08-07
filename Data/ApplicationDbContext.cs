using AuctionHouse.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Bid> Bids { get; set; }
    }
}
