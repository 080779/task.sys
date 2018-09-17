using IMS.DTO;
using System;
using System.Threading.Tasks;

namespace IMS.IService
{
    public interface IUserService:IServiceSupport
    {
        Task<long> AddAsync(string name, string password, string nickName, string avatarUrl);
        Task<bool> UpdateInfoAsync(long id,string nickName, string headpic);
        Task<long> DeleteAsync(long id);
        Task<bool> FrozenAsync(long id);
        Task<long> ResetPasswordAsync(long id, string password);
        Task<long> CheckLoginAsync(string name, string password);
        Task<long> CheckUserMobileAsync(long id, string mobile);
        Task<long> CheckUserNameAsync(string name);
        bool CheckUserId(long id);
        Task<long> BindInfoAsync(long id, string mobile,string trueName,string wechatPayCode,string aliPayCode);
        Task<long> ResetBindInfoAsync(long id, string mobile, string trueName, string wechatPayCode, string aliPayCode);
        string GetPayInfo(long userId,string payTypeName);
        Task<decimal> GetAmountByIdAsync(long id);
        Task<string> GetMobileByIdAsync(long id);
        Task<UserDTO> GetModelAsync(long id);
        Task<UserSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize);
    }
    public class UserSearchResult
    {
        public UserDTO[] Users { get; set; }
        public long PageCount { get; set; }
    }
}
