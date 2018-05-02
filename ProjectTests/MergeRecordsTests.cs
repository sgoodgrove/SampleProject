using System.Collections.Generic;
using System.Linq;

using FluentAssertions;
using Moq;
using WebApplication1.Models;
using WebApplication1.Services;
using Xunit;

namespace ProjectTests
{
    public class MergeRecordsTests
    {
        private readonly IUserRepository userRepository;

        private readonly IUserServices userServices = new UserServices();

        public MergeRecordsTests()
        {
            var users = new List<User>
            {
                new User
                {
                    Address = new Address
                    {
                        City = "Sheffield",
                        GeoCoords = new GeoCoords
                        {
                            Lat = 0.0M,
                            Lng = 0.0M
                        },
                        Street = "Acacia Avenue",
                        Suite = "Suite 1",
                        ZipCode = "ZIP 1"
                    },
                    Company = new Company
                    {
                        Bs = "Some Bs 1",
                        CatchPhrase = "What is a hotspot not?",
                        Name = "Company 1"
                    },
                    Email = "user1@company1.com",
                    Id = 1,
                    Name = "User 1",
                    Phone = "010101",
                    UserName = "User1",
                    Website = "www.user1.com"
                },
                new User
                {
                    Address = new Address
                    {
                        City = "London",
                        GeoCoords = new GeoCoords
                        {
                            Lat = 0.1M,
                            Lng = 0.1M
                        },
                        Street = "Baker Street",
                        Suite = "Suite 2",
                        ZipCode = "ZIP 2"
                    },
                    Company = new Company
                    {
                        Bs = "Some Bs 2",
                        CatchPhrase = "Good game good game",
                        Name = "Company 2"
                    },
                    Email = "user2@company2.com",
                    Id = 2,
                    Name = "User 2",
                    Phone = "020202",
                    UserName = "User2",
                    Website = "www.user2.com"
                },
            };

            var thing = new Mock<IUserRepository>();
            thing.Setup(x => x.GetUser(It.IsAny<int>())).Returns((int x) => users.First(u => u.Id == x));
            userRepository = thing.Object;
        }

        [Fact]
        public void MergeUserRecords_AllFieldsSpecified_Overwritten()
        {
            var userService = new UserServices();
            var u1 = userRepository.GetUser(1);
            var u2 = userRepository.GetUser(2);
            var merged = userService.MergeRecords(u1, u2);

            merged.Company.Bs.Should().Be("Some Bs 1");
            merged.Company.Name.Should().Be("Company 1");
            merged.Company.CatchPhrase.Should().Be("What is a hotspot not?");

            merged.Address.City.Should().Be("Sheffield");
            merged.Address.GeoCoords.Lat.Should().Be(0M);
            merged.Address.GeoCoords.Lng.Should().Be(0M);
            merged.Address.Street.Should().Be("Acacia Avenue");
            merged.Address.Suite.Should().Be("Suite 1");
            merged.Address.ZipCode.Should().Be("ZIP 1");

            merged.Email.Should().Be("user1@company1.com");
            merged.Name.Should().Be("User 1");
            merged.Phone.Should().Be("010101");
            merged.UserName.Should().Be("User1");
            merged.Website.Should().Be("www.user1.com");
        }

        [Fact]
        public void MergeUserRecords_BlackFieldsSpecified_Overwritten()
        {
            var userService = new UserServices();
            var u1 = new User
            {
                Id = 1,
                Email = string.Empty,
                Website = string.Empty,
                Address = new Address
                {
                    ZipCode = string.Empty
                }
            };

            var u2 = userRepository.GetUser(2);
            var merged = userService.MergeRecords(u1, u2);

            merged.Company.Bs.Should().Be("Some Bs 2");
            merged.Company.Name.Should().Be("Company 2");
            merged.Company.CatchPhrase.Should().Be("Good game good game");

            merged.Address.City.Should().Be("London");
            merged.Address.GeoCoords.Lat.Should().Be(0.1M);
            merged.Address.GeoCoords.Lng.Should().Be(0.1M);
            merged.Address.Street.Should().Be("Baker Street");
            merged.Address.Suite.Should().Be("Suite 2");
            merged.Address.ZipCode.Should().Be(string.Empty);

            merged.Email.Should().Be(string.Empty);
            merged.Name.Should().Be("User 2");
            merged.Phone.Should().Be("020202");
            merged.UserName.Should().Be("User2");
            merged.Website.Should().Be(string.Empty);
        }

        [Fact]
        public void MergeUserRecords_IdField_NotOverwritten()
        {
            var userService = new UserServices();
            var u1 = userRepository.GetUser(1);
            var u2 = userRepository.GetUser(2);
            var merged = userService.MergeRecords(u1, u2);

            merged.Id.Should().Be(2);
        }

        [Fact]
        public void MergeUserRecords_NoFieldSpecified_NotOverwritten()
        {
            var userService = new UserServices();
            var u1 = new User { Id = 1 };
            var u2 = userRepository.GetUser(2);
            var merged = userService.MergeRecords(u1, u2);

            merged.Company.Bs.Should().Be("Some Bs 2");
            merged.Company.Name.Should().Be("Company 2");
            merged.Company.CatchPhrase.Should().Be("Good game good game");

            merged.Address.City.Should().Be("London");
            merged.Address.GeoCoords.Lat.Should().Be(0.1M);
            merged.Address.GeoCoords.Lng.Should().Be(0.1M);
            merged.Address.Street.Should().Be("Baker Street");
            merged.Address.Suite.Should().Be("Suite 2");
            merged.Address.ZipCode.Should().Be("ZIP 2");

            merged.Email.Should().Be("user2@company2.com");
            merged.Name.Should().Be("User 2");
            merged.Phone.Should().Be("020202");
            merged.UserName.Should().Be("User2");
            merged.Website.Should().Be("www.user2.com");
        }
    }
}