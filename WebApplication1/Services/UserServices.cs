using System;

using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserServices : IUserServices
    {
        public User MergeRecords(User sourceUserRecord, User targetUserRecord)
        {
            var u1 = sourceUserRecord;
            var u2 = targetUserRecord;
            Func<Func<User, string>, string> merge = (u) => (u(u1) == null ? u(u2) : u(u1));

            return new User
            {
                Address = new Address
                {
                    City = merge((u) => u.Address?.City),
                    GeoCoords = u1.Address?.GeoCoords ?? u2.Address?.GeoCoords,
                    Street = merge((u) => u.Address?.Street),
                    Suite = merge((u) => u.Address?.Suite),
                    ZipCode = merge((u) => u.Address?.ZipCode)
                },
                Company = new Company
                {
                    Bs = merge((u) => u.Company?.Bs),
                    CatchPhrase = merge((u) => u.Company?.CatchPhrase),
                    Name = merge((u) => u.Company?.Name)
                },
                Email = merge((u) => u.Email),
                Name = merge((u) => u.Name),
                Id = targetUserRecord.Id,
                Phone = merge((u) => u.Phone),
                UserName = merge((u) => u.UserName),
                Website = merge((u) => u.Website)
            };
        }
    }
}
