using BlogApp.Data.Abstract;
using BlogApp.Data.Concreate.EfCore;
using BlogApp.Entity;

namespace BlogApp.Data.Concreate
{
    public class EfUserRepository:IUserRepository{
        private BlogContext _context;

        public EfUserRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<User> Users => _context.Users;

        public void CreateUser(User user){
            _context.Users.Add(user);
            _context.SaveChanges();
        }
    }


}