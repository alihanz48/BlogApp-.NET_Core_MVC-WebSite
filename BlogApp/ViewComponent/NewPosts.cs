using System.Reflection;
using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.ViewComponents{
    public class NewPosts:ViewComponent{

        private IPostRepository _postRepositry;
        public NewPosts(IPostRepository postRepository)
        {
            _postRepositry=postRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(){
            return View(
                await
                _postRepositry
                .Posts
                .OrderByDescending(p=>p.PublishedOn)
                .Take(5)
                .ToListAsync());
        }
    }
}