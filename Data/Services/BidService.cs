using AuctionHouse.Models;

namespace AuctionHouse.Data.Services
{
    public class BidService : IBidService
    {
        private readonly ApplicationDbContext _context;

        public BidService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Bid bid)
        {
            _context.Bids.Add(bid);
            await _context.SaveChangesAsync();
        }
    }
}
