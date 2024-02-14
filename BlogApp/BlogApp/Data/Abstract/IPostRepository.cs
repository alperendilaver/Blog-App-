using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository{
        IQueryable<Post> Posts {get;} //IQueryable listenin bir versiyonu context üzerinden bütün postları aldığımız zaman ekstra filtrelemeye devam edebileceğiz IEnumerableda bütün veriler alınır onun üzerinden filtrelenir ancak IQueryable olunca veritabanında filtreleme yapılıyor sanırım

        void CreatePost(Post post);
        void EditPost(PostEditViewModel post);
    }
}