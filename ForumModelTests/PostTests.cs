using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForumModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace ForumModel.Tests
{
    [TestClass()]
    public class PostTests
    {
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
        [TestMethod()]
        public void AddNewPinComment_OK_Test()
        {
            var post = GetTestPost();

            post.AddNewPinComment(12);

            Assert.AreEqual(12, post.PinComment.Id);
            Assert.AreEqual("reply2", post.PinComment.Description);
        }

        [TestMethod()]
        public void AddNewPinComment_FAIL_Test()
        {
            var post = GetTestPost();

            var ex = Assert.ThrowsException<ArgumentException>(
                () =>  post.AddNewPinComment(999));

            Assert.AreEqual("Comentario no encontrado.", ex.Message);
        }

        [TestMethod()]
        public void EditTitle_OK_Test()
        {
            var post = GetTestPost();

            post.EditTitle("new title", 123);

            Assert.AreEqual("new title", post.Title);
        }

        [TestMethod()]
        public void EditTitle_FAIL_WrongUserId_Test()
        {
            var post = GetTestPost();

            var ex = Assert.ThrowsException<ArgumentException>(
                () => post.EditTitle("new title", 999));

            Assert.AreEqual("userId no coincide con el del autor.", ex.Message);
        }

        [TestMethod()]
        public void EditTitle_FAIL_EmptyTitle_Test()
        {
            var post = GetTestPost();

            var ex = Assert.ThrowsException<ArgumentException>(
                () => post.EditTitle("", 123));

            Assert.AreEqual("El contenido no puede ser vacio.", ex.Message);
        }
    }
}