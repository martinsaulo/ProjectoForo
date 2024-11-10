using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;
using ForumModel.Entities;

namespace ForumModel.Tests
{
    [TestClass()]
    public class PostTests
    {/*
        private Post GetTestPost()
        {
            var author = new ForumUser("test", "test@mail.com", "qwerty12345");
            author.UserId = 123;

            var post = Post.CreatePost("LorempIpsum", "Test Title", author);

            Comment reply1 = new Comment()
            {
                Id = 11,
                Description = "reply1",
                Author = author,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
            };
            Comment reply2 = new Comment()
            {
                Id = 12,
                Description = "reply2",
                Author = author,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
            };

            post.Replies = new List<Comment> { reply1, reply2 };

            return post;
        }
        */
    }
}