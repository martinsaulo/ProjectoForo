using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumModel.Entities;
using ForumModel.Context;

namespace ForumModel.Tests
{
    [TestClass()]
    public class CommentTests
    {/*
        [TestInitialize()]
        public void GetComment()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            ForumUser testUser = new ForumUser("test_user", "test_user@email.com", "contraseña123");

            var comment1 = new Comment()
            {
                Description = "comment_1",
                Author = testUser,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
                Replies = new List<Comment>()
            };

            var comment2 = new Comment()
            {
                Description = "comment_2",
                Author = testUser,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
                Replies = new List<Comment>()
            };

            var comment3 = new Comment()
            {
                Description = "comment_3",
                Author = testUser,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
            };

            comment2.Replies.Add(comment3);

            comment1.Replies.Add(comment2);
            
            db.Comments.Add(comment1);

            db.SaveChanges();

        }
        [TestCleanup()]
        public void Cleanup()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var testUser = db.Users.FirstOrDefault(u => u.Nickname == "test_user");
            var comments = db.Comments.Where(x => x.Author.Nickname == "test_user");

            db.Comments.RemoveRange(comments);
            db.Users.Remove(testUser);
            db.SaveChanges();
        }*/
        
    }
}