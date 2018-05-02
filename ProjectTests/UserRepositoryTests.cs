using System;
using System.IO;
using System.Linq;

using FluentAssertions;
using WebApplication1.Models;
using WebApplication1.Services;
using Xunit;

namespace ProjectTests
{
    [Trait("Category", "Integration Tests")]
    public class UserRepositoryTests
    {
        public UserRepositoryTests()
        {
        }

        [Fact]
        public void CreateUser_DuplicatedName_Fails()
        {
            var userRepository = GetMockRepository();
            var user1 = new User { UserName = "Steven" };
            var user2 = new User { UserName = "Steven" };

            Action<User> create = (u) => userRepository.CreateUser(u);

            userRepository.Invoking(r => r.CreateUser(user1)).Should().NotThrow<InvalidOperationException>();
            userRepository.Invoking(r => r.CreateUser(user2)).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void CreateUser_GetUser_Matches()
        {
            var userRepository = GetMockRepository();
            var newUser = new User { UserName = "Steven", Name = "Steve" };

            var userId = userRepository.CreateUser(newUser);

            var retrievedUser = userRepository.GetUser(userId);

            retrievedUser.Should().NotBeNull();
            retrievedUser.UserName.Should().Be("Steven");
            retrievedUser.Name.Should().Be("Steve");
        }

        [Fact]
        public async void DeleteUser_UserRemoved()
        {
            var userRepository = GetMockRepository();
            var initialUsers = (await userRepository.GetAllUsers()).ToArray();
            initialUsers.Should().Contain((u) => u.Id == 1, "User record should exist prior to deletion");
            userRepository.DeleteUser(1);

            var allUsers = (await userRepository.GetAllUsers()).ToArray();
            allUsers.Should().NotContain((u) => u.Id == 1, "User record should not exist after deletion");

            allUsers.Length.Should().Be(initialUsers.Length - 1);
        }

        [Fact]
        public async void GetAllUsers_ReturnsExpected()
        {
            var userRepository = GetMockRepository();
            var allUsers = (await userRepository.GetAllUsers()).ToArray();

            allUsers.Count().Should().Be(3);
            allUsers.Should().Contain((u) => u.UserName == "User 1");
            allUsers.Should().Contain((u) => u.UserName == "User 2");
            allUsers.Should().Contain((u) => u.UserName == "User 3");
        }

        [Fact]
        public async void UpdateUser_AllFields_Updated()
        {
            var userRepository = GetMockRepository();

            var u1 = (await userRepository.GetAllUsers()).First((u) => u.UserName == "User 1");
            u1.Name = "Updated User 1";
            userRepository.UpdateUser(u1);

            var retrievedRecord = userRepository.GetUser(u1.Id);

            retrievedRecord.Name.Should().Be("Updated User 1");
        }

        private IUserRepository GetMockRepository()
        {
            var dbStream = new MemoryStream();
            var userRepository = new UserRepository(new LiteDB.LiteDatabase(dbStream), new UserServices());

            userRepository.CreateUser(new User { UserName = "User 1" });
            userRepository.CreateUser(new User { UserName = "User 2" });
            userRepository.CreateUser(new User { UserName = "User 3" });

            return userRepository;
        }
    }
}