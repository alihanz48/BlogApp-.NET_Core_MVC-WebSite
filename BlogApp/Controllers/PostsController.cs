using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{

  public class PostsController : Controller
  {

    private IPostRepository _postrepository;
    private ICommentRepository _commentrepository;

    private ITagRepository _tagrepository;

    public PostsController(IPostRepository postrepository, ICommentRepository commentRepository, ITagRepository tagrepository)
    {
      _postrepository = postrepository;
      _commentrepository = commentRepository;
      _tagrepository = tagrepository;

    }


    public async Task<IActionResult> Index(string? tag)
    {
      var claims = User.Claims;

      var posts = _postrepository.Posts.Include(t => t.Tags).Where(x=>x.IsActive==true);

      if (!string.IsNullOrEmpty(tag))
      {
        posts = posts.Where(x => x.Tags.Any(t => t.Url == tag)).Include(t => t.Tags);
      }

      return View(
        new PostsViewModel
        {
          Posts = await posts.ToListAsync()
        }
      );
    }

    public async Task<IActionResult> Details(string url)
    {
      return View(
        await
        _postrepository
        .Posts
        .Include(X=>X.User)
        .Include(x => x.Tags)
        .Include(x => x.Comments)
        .ThenInclude(x => x.User)
        .FirstOrDefaultAsync(p => p.Url == url));
    }

    public JsonResult AddComment(int PostID, string Text)
    {
      var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
      var userName = User.FindFirstValue(ClaimTypes.Name);
      var avatar = User.FindFirstValue(ClaimTypes.UserData);

      var entity = new Comment
      {
        Text = Text,
        PublishedOn = DateTime.Now,
        PostID = PostID,
        UserID = int.Parse(userId ?? "")
      };
      _commentrepository.CreateComment(entity);

      return Json(new
      {
        userName,
        Text,
        entity.PublishedOn,
        avatar
      });
    }

    [Authorize]
    public IActionResult Create()
    {
      return View();
    }

    [Authorize]
    [HttpPost]
    public IActionResult Create(PostCreateViewModel model)
    {

      if (ModelState.IsValid)
      {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        _postrepository.CreatePost(
         new Post
         {
           Title = model.title,
           Description = model.Description,
           Content = model.Content,
           Url = model.Url,
           UserId = int.Parse(userId ?? ""),
           PublishedOn = DateTime.Now,
           Image = "1.jpg",
           IsActive = false,
         }
        );

        return RedirectToAction("Index");

      }
      return View(model);
    }

    [Authorize]
    public async Task<IActionResult> List()
    {
      var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
      var role = User.FindFirstValue(ClaimTypes.Role);
      var posts = _postrepository.Posts;

      if (String.IsNullOrEmpty(role))
      {
        posts = posts.Where(x => x.UserId == userid);
      }

      return View(await posts.Include(x => x.User).ToListAsync());
    }

    [Authorize]
    public IActionResult Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }

      var post = _postrepository.Posts.Include(x=>x.Tags).FirstOrDefault(x => x.PostID == id);

      if (post == null)
      {
        return NotFound();
      }

      ViewBag.tags = _tagrepository.Tags.ToList();

      return View(new PostCreateViewModel
      {
        PostID = post.PostID,
        title = post.Title,
        Description = post.Description,
        Content = post.Content,
        Url = post.Url,
        IsActive = post.IsActive,
        Tags=post.Tags
      }
      );
    }

    [Authorize]
    [HttpPost]
    public IActionResult Edit(PostCreateViewModel model, int[] tagIDs)
    {
      
      if (ModelState.IsValid)
      {

        var postUpdate = new Post
        {
          PostID = model.PostID,
          Title = model.title,
          Description = model.Description,
          Content = model.Content,
          Url = model.Url,
          IsActive = model.IsActive,
        };

        _postrepository.EditPost(postUpdate,tagIDs);
        return RedirectToAction("List");
      }

      return View(model);
    }
  }
}