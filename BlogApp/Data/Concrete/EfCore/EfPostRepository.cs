using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlogApp.Data.Concrete
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;

        public EfPostRepository(BlogContext context)
        {
            _context = context;
        }

        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void EditPost(Post post, int[] tagIDs)
        {
            var entity = _context.Posts.Include(i=>i.Tags).FirstOrDefault(x => x.PostID == post.PostID);
            if (entity != null)
            {
                entity.Title = post.Title;
                entity.Description = post.Description;
                entity.Content = post.Content;
                entity.Url = post.Url;
                entity.IsActive = post.IsActive;
                entity.Tags = _context.Tags.Where(tag => tagIDs.Contains(tag.TagID)).ToList();
                _context.SaveChanges();
            }
        }
    }
}