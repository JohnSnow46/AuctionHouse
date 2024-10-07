using AuctionHouse.Models;

namespace AuctionHouse.Data.Services
{
    public interface ICommentService
    {
        Task AddComment(Comment comment);
        Task RemoveComment(Comment comment);
    }
}
