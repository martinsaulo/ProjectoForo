using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForumModel.Entities;
using ForumModel.Context;
using ForumAPI.Controllers;
using ForumModel.Repositories;
using ForumModel.Repositories.Contracts;


namespace ForumModel.Tests
{

    [TestClass()]
    public class UserTests
    {
        private readonly UserController _controller;
        private readonly IUserRepository _mockRepository;
        public UserTests(UserController userController) 
        {

        }
        [TestInitialize()]
        public void Initialize()
        {


        }

        [TestMethod()]
        public void test()
        {
            //_controller.
        }

        /*

        [TestMethod()]
        public void isValidPassword_FAIL_MaxLength_Test()
        {
            string password = "Lorem_ipsum_dolor_sit_amet,_consectetur_efficitur1";

            var ex = Assert.ThrowsException<ArgumentException>(
                () => ForumUser.isValidPassword(password));

            Assert.AreEqual(ex.Message, "La cotraseña debe contener entre 8 y 32 caracteres.");
        }*/
    }
}