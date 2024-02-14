using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concreate.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class UsersController:Controller{
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
    
        public IActionResult Login(){
            if(User.Identity!.IsAuthenticated)
                return RedirectToAction("Index","Posts");
            
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Login(LoginViewModel model){
            
            if(ModelState.IsValid){
                var isUser = _userRepository.Users.FirstOrDefault(x=>x.Email == model.Email && x.Password == model.Password);
                if(isUser!=null){
                    var userClaims = new List<Claim>();
                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier,isUser.UserId.ToString()));
                    userClaims.Add(new Claim(ClaimTypes.Name,isUser.Name ?? ""));
                    
                    userClaims.Add(new Claim(ClaimTypes.GivenName,isUser.UserName ?? ""));

                    userClaims.Add(new Claim(ClaimTypes.UserData,isUser.Image??""));

                    if(isUser.Email == "alperen@gmail.com"){
                        userClaims.Add(new Claim(ClaimTypes.Role,"admin"));
                    }
                    var claimsIdentity = new ClaimsIdentity(userClaims,CookieAuthenticationDefaults.AuthenticationScheme);

                       var authProperties = new AuthenticationProperties{
                        IsPersistent = true//kullanıcıyı hatırlama
                    };
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),authProperties);

                    return RedirectToAction("Index","Posts");
                }
                else{   
                ModelState.AddModelError("","Kullanıcı Adı veya şifre yanlış");

            }
            }
    
            return View(model);
        }

        public IActionResult Register(){
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model){
            if(ModelState.IsValid){
                //var user = await _userRepository.Users.FirstOrDefaultAsync(x=>x.Email == model.Email); aşağıda da bunun null olma durumunu kontrol ederek işlemler yaparız bu da bi yol
                if(!_userRepository.Users.Any(u=>u.Email==model.Email)){

                if(model.imageFile!=null){
                    var extension = Path.GetExtension(model.imageFile.FileName);
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
                        
                    using(var stream = new FileStream(path,FileMode.Create)){
                        await model.imageFile.CopyToAsync(stream);
                    }
                    model.Image = randomFileName; 
                }
                _userRepository.CreateUser(new User{
                    UserName = model.UserName,
                    Email = model.Email,
                    Name = model.Name,
                    Password = model.Password,
                    Image = model.Image
                });
                var userClaims = new List<Claim>();
                userClaims.Add(new Claim(ClaimTypes.Name,model.Name ?? ""));
                userClaims.Add(new Claim(ClaimTypes.GivenName,model.UserName ?? ""));
                userClaims.Add(new Claim(ClaimTypes.UserData,model.Image??""));

                    var claimsIdentity = new ClaimsIdentity(userClaims,CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties{
                        IsPersistent = true//kullanıcıyı hatırlama
                    };
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                   
                return RedirectToAction("Login","Users");
                }
                else{
                    ModelState.AddModelError("","Kullanıcı mevcut");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            return RedirectToAction("Login");
        }
        public IActionResult Profile(string username){

            if(string.IsNullOrEmpty(username))
                return NotFound();
            var model = _userRepository.Users
            .Include(X=>X.Post)
            .Include(x=>x.Comments)
            .ThenInclude(x=>x.Post)
            .FirstOrDefault(x=>x.UserName==username);
            if(model==null)
                return NotFound();
    
            return View(model);
        }
    }
}
