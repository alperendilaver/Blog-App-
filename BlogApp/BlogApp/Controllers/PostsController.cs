using System.ComponentModel;
using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concreate.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController:Controller{
        private IPostRepository _postRepository;
        
        private ICommentRepository _commentRepository;
        private ITagRepository _tagRepository;
        private IUserRepository _userRepository;
      
        public PostsController(IPostRepository postRepository,ICommentRepository commentRepository,ITagRepository tagRepository,IUserRepository userRepository){
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _tagRepository =tagRepository;
            _userRepository = userRepository;
        }
        public async Task< IActionResult> Index(string tag){
            var claims = User.Claims;
            
            var posts=_postRepository.Posts;
            if(!string.IsNullOrEmpty(tag)){
                posts=posts.Where(x=>x.Tags.Any(t=>t.Url==tag));
            }
            
            return View(new PostwithTagViewModel{ Posts = await posts.ToListAsync()});
        }
        public async Task<IActionResult> Details(string
         url){  
                
            return View(await _postRepository
            .Posts.
            Include(x=>x.User)
            .Include(x=>x.Tags)
            .Include(x=>x.Comments)
            .ThenInclude(x=>x.User)
            .FirstOrDefaultAsync(p=>p.Url==url));
        }

        [HttpPost]
        public JsonResult AddComment(int PostId,string Text){
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.GivenName);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);
            var entity = new Comment{
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = Int32.Parse(userId ?? "")
            };
            _commentRepository.CreateComment(entity);
            //return Redirect("details/"+Url);
            //return RedirectToRoute("post_details",new {url=Url});//program.cs deki routedaki name göre redirect yapar ilk "url" routedaki değişken

            return Json(new{
                userName,
                Text,
                entity.PublishedOn,
                avatar
                }
            );
        }
        [Authorize]
        public IActionResult Create(){
            var tags=new SelectList(_tagRepository.Tags,nameof(Tag.TagId), nameof(Tag.Text));
            ViewBag.Tags=tags;
            var UserIdClaim=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ViewBag.UserId=UserIdClaim;
            var user=new SelectList(_userRepository.Users,"UserId", "UserName");
            ViewBag.Tags=tags;
            ViewBag.Users = user;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel model){
            if(model == null)
                return NotFound();
            
            if(ModelState.IsValid){
                List<int> selectedTags =model.tagsId;
                List<Tag> tags = _tagRepository.GetTagsByIds(selectedTags);
                
                if(model.imageFile!=null){
                    var extension = Path.GetExtension(model.imageFile.FileName);
                    var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                    var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
                        
                    using(var stream = new FileStream(path,FileMode.Create)){
                        await model.imageFile.CopyToAsync(stream);
                    }
                    model.image = randomFileName; 
                }

                _postRepository.CreatePost(new Post{
                    Context=model.Context,
                    Title=model.Title,
                    IsActive=model.IsActive,
                    UserId=model.UserId,
                    Tags = tags,
                    Image = model.image,
                    Url=model.Title
                });
                return RedirectToAction("Index");

            }
            return View(model);
        }
        [Authorize]
        public IActionResult Edit(int id){
            var tags=new SelectList(_tagRepository.Tags,nameof(Tag.TagId), nameof(Tag.Text));

            ViewBag.Tags=tags;
            var post = _postRepository.Posts.Include(x=>x.Tags).
            Include(x=>x.User).FirstOrDefault(x=>x.PostId==id);
            if(post!=null){
                return View(new PostEditViewModel{
                Context=post.Context,
                Image=post.Image,
                Tags=post.Tags,
                Title =post.Title,
                IsActive = post.IsActive,
                User=post.User,
                PostId=post.PostId
                });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task< IActionResult> Edit(PostEditViewModel model){
            if(model!=null){
                var post=await _postRepository.Posts.FirstOrDefaultAsync(x=>x.PostId==model.PostId);
                if(post!=null){
                ModelState.Remove("User");
                if(ModelState.IsValid){
      
                    if(!model.changeImage){
                        if(model.imageFile!=null){
                            var extension = Path.GetExtension(model.imageFile.FileName);
                            var randomFileName = string.Format($"{Guid.NewGuid().ToString()}{extension}");
                            var path=Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/img",randomFileName);
                            using(var stream = new FileStream(path,FileMode.Create)){
                                await model.imageFile.CopyToAsync(stream);
                            }
                        model.Image = randomFileName; 
                        }
                    }
                   
                List<int> selectedTags =model.tagsId;
                List<Tag> tags = _tagRepository.GetTagsByIds(selectedTags);
                model.Tags=tags;
                
                _postRepository.EditPost(model);
                }
                }
                return RedirectToAction("Index");
            }
            
            return View();
        }
        [Authorize]
        public async Task< IActionResult> List(){
            var userId= int.Parse( User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role = User.FindFirstValue(ClaimTypes.Role);
            var posts = _postRepository.Posts;

            if(string.IsNullOrEmpty(role))  
                posts=posts.Where(x=>x.UserId==userId);//admin değilse sadece kendi postlarını görsün

            

            return View(await posts.ToListAsync());
        }
        public IActionResult Delete(int id){
            
            return View();
        }
    }
}