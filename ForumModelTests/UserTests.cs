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
    public class UserTests
    {
        [TestInitialize()]
        public void GetUser()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            ForumUser testUser = new ForumUser("test_nombre", "test@email.com", "contraseña123");

            db.Users.Add(testUser);
            db.SaveChanges();

        }
        [TestCleanup()]
        public void Cleanup()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var testUser = db.Users.FirstOrDefault(u => u.Nickname == "test_nombre");

            db.Users.Remove(testUser);
            db.SaveChanges();
        }

        [TestMethod()]
        public void isValidNickname_OK_Test()
        {
            string nickname = "prueba_nick";
            bool flag = ForumUser.isValidNickname(nickname);

            Assert.IsTrue(flag);
        }

        [TestMethod()]
        public void isValidNickname_FAIL_AlredyExists_Test()
        {
            string nickname = "test_nombre"; 

            var ex = Assert.ThrowsException<ArgumentException>(
                () => ForumUser.isValidNickname(nickname));

            Assert.AreEqual(ex.Message, "Nickname ya en uso.");
        }

        [TestMethod()]
        public void isValidEmail_OK_Test()
        {
            string email = "test2@email.com";
            bool flag = ForumUser.isValidEmail(email);

            Assert.IsTrue(flag);
        }

        [TestMethod()]
        public void isValidEmail_FAIL_AlredyExist_Test()
        {
            string email = "test@email.com";

            var ex = Assert.ThrowsException<ArgumentException>(
                () => ForumUser.isValidEmail(email));

            Assert.AreEqual(ex.Message, "Email ya registrado.");
        }

        [TestMethod()]
        public void isValidEmail_FAIL_InvalidFormat_Test1()
        {
            string email = "test@email";

            var ex = Assert.ThrowsException<ArgumentException>(
                () => ForumUser.isValidEmail(email));

            Assert.AreEqual(ex.Message, "Formato de email invalido.");
        }

        [TestMethod()]
        public void isValidEmail_FAIL_InvalidFormat_Test2()
        {
            string email = "test_email.com";

            var ex = Assert.ThrowsException<ArgumentException>(
                () => ForumUser.isValidEmail(email));

            Assert.AreEqual(ex.Message, "Formato de email invalido.");
        }

        [TestMethod()]
        public void isValidPassword_OK_Test()
        {
            string password = "qwerty1234";
            bool flag = ForumUser.isValidPassword(password);

            Assert.IsTrue(flag);
        }

        [TestMethod()]
        public void isValidPassword_FAIL_NoDigit_Test()
        {
            string password = "qwertyuiop";

            var ex = Assert.ThrowsException<ArgumentException>(
                () => ForumUser.isValidPassword(password));

            Assert.AreEqual(ex.Message, "La contraseña debe contener al menos un número.");
        }

        [TestMethod()]
        public void isValidPassword_FAIL_MinLength_Test()
        {
            string password = "qwe1";

            var ex = Assert.ThrowsException<ArgumentException>(
                () => ForumUser.isValidPassword(password));

            Assert.AreEqual(ex.Message, "La cotraseña debe contener entre 8 y 32 caracteres.");
        }

        [TestMethod()]
        public void isValidPassword_FAIL_MaxLength_Test()
        {
            string password = "Lorem_ipsum_dolor_sit_amet,_consectetur_efficitur1";

            var ex = Assert.ThrowsException<ArgumentException>(
                () => ForumUser.isValidPassword(password));

            Assert.AreEqual(ex.Message, "La cotraseña debe contener entre 8 y 32 caracteres.");
        }
    }
}