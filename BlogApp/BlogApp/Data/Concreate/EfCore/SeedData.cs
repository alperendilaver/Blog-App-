using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concreate.EfCore
{
    public static class SeedData{
        public static void TestVerileriniDoldur(IApplicationBuilder app) {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();
            if(context != null){
                if(context.Database.GetPendingMigrations().Any()){
                    context.Database.Migrate();
                }
                if(!context.Tags.Any()){
                    context.Tags.AddRange(
                    new Tag{Text ="Web Programlama" ,Url="web-programlama", Color = TagColor.primary},
                        
                    new Tag{Text ="Backend",Url="backend" ,Color=TagColor.danger},
                    
                    new Tag{Text ="Frontend",Url = "frontend" ,Color=TagColor.warning},
                    
                    new Tag{Text ="Makine Öğrenmesi",Url="makine-ogrenmesi" , Color = TagColor.secondary},
                    
                    new Tag{Text ="FullStack",Url="fullstack",Color=TagColor.success }
                    );
                    context.SaveChanges();
                }
                if(!context.Users.Any()){
                    context.Users.AddRange(
                        new User {UserName = "AlperenDilaver",Image="p1.jpg",Email="alperen@gmail.com",Name="Alperen",Password="1453"},
                        new User {UserName = "TuanaKarasol",Image="p2.jpg",Email="tuana@gmail.com",Name="Tuana",Password="2023"}
                    );
                    context.SaveChanges();
                }
                if(!context.Posts.Any()){
                    context.Posts.AddRange(
                        new Post{
                            Title = "Asp.net Core",
                            Context = "Asp.net Core dersleri",
                            Url="aspnet-core",
                            IsActive = true,
                            PublishedOn =DateTime.Now.AddDays(-10),
                            Tags = context.Tags.Take(3).ToList(),
                            Image="1.jpg",
                            UserId = 1,
                            Comments =new List<Comment>{
                                new Comment{Text ="Faydalı",PublishedOn = new DateTime(),UserId=1},
                                new Comment{Text ="Başarılı Kurs",PublishedOn = new DateTime(),UserId=2}
                            }
                        },
                        new Post{
                            Title = "Angular",
                            Context = "Angular dersleri",
                            Url="angular",
                            IsActive = true,
                            PublishedOn =DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            Image="2.jpg",                           
                            UserId = 1
                        },
                        new Post{
                            Title = "Php",
                            Context = "Php dersleri",
                            Url="php",
                            IsActive = true,
                            PublishedOn =DateTime.Now.AddDays(-2),
                            Tags = context.Tags.Take(2).ToList(),
                            Image="2.jpg",                           
                            UserId =2
                        },
                        new Post{
                            Title = "Python",
                            Context = "Python dersleri",
                            Url="python",
                            IsActive = true,
                            PublishedOn =DateTime.Now.AddDays(-30),
                            Tags = context.Tags.Take(3).ToList(),
                            Image="3.jpg",                           
                            UserId = 1
                        },
                        new Post{
                            Title = "Laravel",
                            Context = "Laravel dersleri",
                            Url="laravel",
                            IsActive = true,
                            PublishedOn =DateTime.Now.AddDays(-20),
                            Tags = context.Tags.Take(2).ToList(),
                            Image="2.jpg",                           
                            UserId = 2
                        },
                        new Post{
                            Title = "React.js",
                            Context = "React.js dersleri",
                            Url="reactjs",
                            IsActive = true,
                            PublishedOn =DateTime.Now.AddDays(-5),
                            Tags = context.Tags.Take(2).ToList(),
                            Image="3.jpg",
                            UserId = 2
                        }
                    );
                    context.SaveChanges();
                }
            }
         }
    }
}