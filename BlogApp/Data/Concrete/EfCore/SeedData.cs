using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace BlogApp.Data.Concrete.EfCore
{
    public static class SeedData
    {

        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.AddRange(
                        new Tag { Text = "Web Programlama",Url="web-programlama",Color=TagColors.warning},
                        new Tag { Text = "Backend",Url="backend" ,Color=TagColors.info},
                        new Tag { Text = "Frontend",Url="frontend",Color=TagColors.success },
                        new Tag { Text = "Fullstack",Url="fullstack" ,Color=TagColors.secondary},
                        new Tag { Text = "Php",Url="php",Color=TagColors.primary}
                    );
                    context.SaveChanges();
                }

                if (!context.Users.Any())
                {
                    context.Users.AddRange(
                        new User { UserName = "sadikturan",Image="p1.jpg",Name="Sadık Turan",Email="info@sadikturan.com",Password="123456"},
                        new User { UserName = "cinarturan",Image="p2.jpg",Name="Çınar Turan",Email="info@cinarturan.com",Password="123456"}
                    );
                    context.SaveChanges();
                }

                if (!context.Posts.Any())
                {
                    context.Posts.AddRange(
                        new Post { 
                            Title = "Asp.net Core", 
                            Description="Asp.net core dersleri",
                            Content = "Asp.net core dersleri",
                            Url="aspnet-core",
                            IsActive = true, 
                            PublishedOn = DateTime.Now.AddDays(-10), 
                            Tags=context.Tags.Take(3).ToList(),
                            Image="1.jpg",
                            UserId = 1,
                            Comments=new List<Comment>{
                                new Comment{Text="İyi bir kurs",PublishedOn=DateTime.Now.AddHours(-10),UserID=1},
                                new Comment{Text="çok faydalandığım bir kurs",PublishedOn=DateTime.Now,UserID=2},
                            }
                        },
                        new Post { 
                            Title = "Php", 
                            Description="php dersleri",
                            Content = "php dersleri", 
                            Url="php",
                            IsActive = true, 
                            PublishedOn = DateTime.Now.AddDays(-20), 
                            Tags=context.Tags.Take(2).ToList(),
                            Image="2.jpg",
                            UserId = 1 
                        },
                        new Post { 
                            Title = "Django", 
                            Content = "Django dersleri", 
                            Description = "Django dersleri",
                            Url="django", 
                            IsActive = true, 
                            PublishedOn = DateTime.Now.AddDays(-30), 
                            Tags=context.Tags.Take(4).ToList(),
                            Image="3.jpg",
                            UserId = 2
                        },
                        new Post { 
                            Title = "React Dersleri", 
                            Description = "React dersleri",
                            Content = "React dersleri",
                            Url="react-dersleri", 
                            IsActive = true, 
                            PublishedOn = DateTime.Now.AddDays(-40), 
                            Tags=context.Tags.Take(4).ToList(),
                            Image="4.jpg",
                            UserId = 2
                        },
                        new Post { 
                            Title = "Angular Dersleri", 
                            Description = "Angular dersleri",
                            Content = "Angular dersleri",
                            Url="angular-dersleri", 
                            IsActive = true, 
                            PublishedOn = DateTime.Now.AddDays(-50), 
                            Tags=context.Tags.Take(4).ToList(),
                            Image="5.jpg",
                            UserId = 2
                        },
                        new Post { 
                            Title = "Web Tasarım", 
                            Description = "Web Tasarım",
                            Content = "Web Tasarım",
                            Url="web-tasarim", 
                            IsActive = true, 
                            PublishedOn = DateTime.Now.AddDays(-60), 
                            Tags=context.Tags.Take(4).ToList(),
                            Image="6.jpg",
                            UserId = 2
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}