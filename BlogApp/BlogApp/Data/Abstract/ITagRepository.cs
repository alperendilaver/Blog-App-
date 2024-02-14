using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface ITagRepository{
        IQueryable<Tag> Tags {get;} //IQueryable listenin bir versiyonu context üzerinden bütün postları aldığımız zaman ekstra filtrelemeye devam edebileceğiz IEnumerableda bütün veriler alınır onun üzerinden filtrelenir ancak IQueryable olunca veritabanında filtreleme yapılıyor sanırım

        void CreateTag(Tag tag);
        List<Tag> GetTagsByIds(List<int> tagIds);
    }
}