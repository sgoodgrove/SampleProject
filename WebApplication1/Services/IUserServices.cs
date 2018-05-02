using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IUserServices
    {
        User MergeRecords(User sourceUserRecord, User targetUserRecord);
    }
}
