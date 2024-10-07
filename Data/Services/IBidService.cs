using AuctionHouse.Models;

namespace AuctionHouse.Data.Services
{
    public interface IBidService
    {
        Task Add(Bid bid);
        IQueryable<Bid> GetAll();
    }
}
