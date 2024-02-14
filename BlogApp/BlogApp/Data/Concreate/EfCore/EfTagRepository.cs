using BlogApp.Data.Abstract;
using BlogApp.Data.Concreate.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concreate
{
    public class EfTagRepository : ITagRepository
    {
        private BlogContext _context;
        public EfTagRepository(BlogContext context)
        {
            _context = context; 
        }
        public IQueryable<Tag> Tags => _context.Tags;

      
        public void CreateTag(Tag tag)
        {
            _context.Add(tag);
            _context.SaveChanges();
        }
        public List<Tag> GetTagsByIds(List<int> tagIds){
            return _context.Tags.Where(x=>tagIds.Contains(x.TagId)).ToList();
        }

    }
}