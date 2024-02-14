using BlogApp.Data.Abstract;
using BlogApp.Data.Concreate.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concreate
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context = context; 
        }
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Add(post);
            _context.SaveChanges();
        }
        public void EditPost(PostEditViewModel model){
            var post=Posts.FirstOrDefault(x=>x.PostId==model.PostId);
            post.Tags=model.Tags;
            post.Title=model.Title;
            post.Context=model.Context;
            post.IsActive=model.IsActive;
            post.Image=model.Image;
            _context.SaveChanges();
        }

    }
}