using BlogApp.Data.Abstract;
using BlogApp.Data.Concreate.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concreate
{
    public class EfCommentRepository : ICommentRepository
    {
        private BlogContext _context;
        public EfCommentRepository(BlogContext context)
        {
            _context = context; 
        }
        public IQueryable<Comment> Comments => _context.Comments;

      
        public void CreateComment(Comment comment)
        {
            _context.Add(comment);
            _context.SaveChanges();
        }

    }
}