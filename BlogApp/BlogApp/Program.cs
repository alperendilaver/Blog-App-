using BlogApp.Data.Abstract;
using BlogApp.Data.Concreate;
using BlogApp.Data.Concreate.EfCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BlogContext>(options=>{
    var config = builder.Configuration;
    var connectionString= config.GetConnectionString("sql_connection");
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<IPostRepository,EfPostRepository>();//ilk sınıf çağırılınca 2.sınıf nesne oluşturulup getiriliyo ,interface repositoryleri tek bir yerden yönetmeyi sağlıyor
builder.Services.AddScoped<ITagRepository,EfTagRepository>();
builder.Services.AddScoped<ICommentRepository,EfCommentRepository>();
builder.Services.AddScoped<IUserRepository,EfUserRepository>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options=>
{options.LoginPath="/Users/Login";
});//cookie kullanacağımızı uygulamaya söyledik

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();//en başa önemli
app.UseAuthentication();//middlewarein kullanılmasını sağlar
app.UseAuthorization();//uygulamanın belli bölümlerini kullanmamızı sağlar
SeedData.TestVerileriniDoldur(app);

//localhost:/posts/url şeklinde başlayan urlleri yönlendirme
app.MapControllerRoute(
    name:"post_details",
    pattern:"posts/details/{url}",
    defaults:new {controller="Posts", action="Details"}
);

app.MapControllerRoute(
    name:"posts_by_tag",
    pattern:"posts/tag/{tag}",
    defaults:new {controller="Posts", action="Index"}
);

app.MapControllerRoute(
    name:"posts_by_tag",
    pattern:"users/profile/{username}",
    defaults:new {controller="Users", action="Profile"}
);

app.MapControllerRoute(
    name:"default",
    pattern:"{controller=Posts}/{action=Index}/{id?}"
);

app.Run();
