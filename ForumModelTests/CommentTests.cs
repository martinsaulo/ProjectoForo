using Microsoft.VisualStudio.TestTools.UnitTesting;
using ForumModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumModel.Tests
{
    [TestClass()]
    public class CommentTests
    {
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
        }
        public Comment GetTestComment()
        {
            var author = new ForumUser("test", "test@mail.com", "qwerty12345");
            author.UserId = 123;
            var comment = new Comment()
            {
                Id = 11,
                Description = "test comment",
                Author = author,
                CreateAt = DateTime.Now,
                LikeCount = 0,
                isEdited = false,
            };

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

            comment.Replies = new List<Comment> { reply1, reply2 };

            return comment;
        }

        [TestMethod()]
        public void GetComment_OK_Test()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            int commentId = db.Comments.SingleOrDefault(x => x.Description == "comment_1").Id;

            var comment = Comment.GetComment(commentId);
            Assert.AreEqual("comment_1", comment.Description);

            var firstReply = comment.Replies.First();
            Assert.AreEqual("comment_2", firstReply.Description);

            var secondReply = firstReply.Replies.First();
            Assert.AreEqual("comment_3", secondReply.Description);
        }

        [TestMethod()]
        public void GetComment_FAIL_InvalidUser_Test()
        {
            var ex = Assert.ThrowsException<NullReferenceException>(
                () => Comment.GetComment(-10));

            Assert.AreEqual("El comentario no existe.", ex.Message);
        }

        [TestMethod()]
        public void Edit_OK_Test()
        {
            var comment = GetTestComment();

            comment.Edit("new content", 123);

            Assert.IsTrue(comment.isEdited);
            Assert.AreEqual("new content", comment.Description);
        }

        [TestMethod()]
        public void Edit_FAIL_WrongUserId_Test()
        {
            var comment = GetTestComment();

            var ex = Assert.ThrowsException<ArgumentException>(
                () => comment.Edit("new content", 999));

            Assert.AreEqual("UserId no coincide con el del autor.", ex.Message);
        }

        [TestMethod()]
        public void Edit_FAIL_EmptyContent_Test()
        {
            var comment = GetTestComment();

            var ex = Assert.ThrowsException<ArgumentException>(
                () => comment.Edit("", 123));

            Assert.AreEqual("El contenido no puede ser vacio.", ex.Message);
        }

        [TestMethod()]
        public void Reply_OK_Test()
        {
            var comment = GetTestComment();

            comment.Reply("new reply", comment.Author);

            Assert.AreEqual("new reply", comment.Replies.Last().Description);
        }

        [TestMethod()]
        public void Reply_FAIL_EmptyContent_Test()
        {
            var comment = GetTestComment();

            var ex = Assert.ThrowsException<ArgumentException>(
                () => comment.Reply("", comment.Author));

            Assert.AreEqual("El contenido no puede ser vacio.", ex.Message);
        }
    }
}