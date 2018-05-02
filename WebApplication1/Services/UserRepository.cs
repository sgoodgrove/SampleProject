using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using LiteDB;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly LiteCollection<User> usersRepository;

        private readonly IUserServices userServices;

        public UserRepository(LiteDatabase repository, IUserServices userServices)
        {
            usersRepository = repository.GetCollection<User>();
            this.userServices = userServices;
        }

        public int CreateUser(User user)
        {
            if (usersRepository.Exists(u => u.UserName == user.UserName))
            {
                var exception = new InvalidOperationException("User already exists.");
                exception.Data["UserName"] = user.UserName;
                throw exception;
            }

            var newUserBson = usersRepository.Insert(user);

            return newUserBson.AsInt32;
        }

        public void DeleteUser(int userId)
        {
            usersRepository.Delete(userId);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await Task.Run(() => usersRepository.FindAll());
        }

        public User GetUser(int userId)
        {
            var user = usersRepository.FindById(userId);
            if (user == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return user;
            }
        }

        public void UpdateUser(User user)
        {
            var existingRecord = this.GetUser(user.Id);
            var mergedRecord = userServices.MergeRecords(existingRecord, user);
            usersRepository.Update(mergedRecord);
        }
    }
}
