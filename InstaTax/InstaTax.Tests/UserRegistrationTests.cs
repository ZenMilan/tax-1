﻿using System;
using InstaTax.Core;
using InstaTax.Core.DomainObjects;
using Moq;
using NUnit.Framework;

namespace InstaTax.Tests{
    [TestFixture]
    public class RegistrationTests
    {
        [Test]
        public void ShouldRegisterUserIfUnique(){
            Mock<IUserRepository> repository = new Mock<IUserRepository>();
            Password password = new Password();
            password.PasswordString = "abc";
            User user = new User("a@a.com", password, repository.Object);
            repository.Setup(rep => rep.Save(user));
            repository.Setup(rep => rep.CheckIfUnique(user)).Returns(true);
            user.Save();
        }

        [Test]
        public void ShouldNotRegisterUserIfNotUnique(){
            Mock<IUserRepository> repository = new Mock<IUserRepository>();
            Password password = new Password();
            password.PasswordString = "abc";
            User user = new User("a@a.com", password, repository.Object);
            repository.Setup(rep => rep.CheckIfUnique(user)).Returns(false);
            Assert.Throws(typeof(DuplicateUserException), user.Save);
        }

        [Test]
        public void ShouldValidateUserId()
        {
            Mock<IUserRepository> repository = new Mock<IUserRepository>();
            Password password = new Password();
            password.PasswordString = "abc";
            string validUserId = "a@a.com";
            string invalidUserId = "aaa";
            User user = new User(validUserId, password, repository.Object);
            Assert.IsTrue(user.IsValidId());

            user = new User(invalidUserId, password, repository.Object);
            Assert.IsFalse(user.IsValidId());
        }

    }

    [TestFixture]
    public class UserTest
    {
        [Test]
        public void ShouldReturnTrueIfTaxPayerIsFemale()
        {
            User taxPayer = new User(0, false, Gender.Female);
            Assert.True(taxPayer.IsFemale());
        }
    }
}