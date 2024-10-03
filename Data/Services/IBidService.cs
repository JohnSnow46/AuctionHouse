using AuctionHouse.Models;

namespace AuctionHouse.Data.Services
{
    public interface IBidService
    {
        Task AddAsync(Bid bid);
    }
}
