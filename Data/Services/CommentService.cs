using AuctionHouse.Models;
using System.Security.Cryptography;

namespace AuctionHouse.Data.Services
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public Task RemoveComment(Comment comment)
        {
            throw new NotImplementedException();
        }
    }
}
