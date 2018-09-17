using IMS.Common;
using IMS.DTO;
using IMS.IService;
using IMS.Service.Entity;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Service.Service
{
    public class UserService : IUserService
    {
        public UserDTO ToDTO(UserEntity entity)
        {
            UserDTO dto = new UserDTO();
            dto.Amount = entity.Amount;
            dto.Name = entity.Name;
            dto.Code = entity.Code;
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.IsEnabled = entity.IsEnabled;
            dto.LevelId = entity.LevelId; 
            dto.Mobile = entity.Mobile;
            dto.NickName = entity.NickName;
            dto.HeadPic = entity.HeadPic;
            dto.WechatPayCode = entity.WechatPayCode;
            dto.AliPayCode = entity.AliPayCode;
            dto.AccountHolder = entity.AccountHolder;
            dto.BankName = entity.BankName;
            dto.BankAccount = entity.BankAccount;
            return dto;
        }

        public async Task<long> AddAsync(string name, string password,string nickName,string avatarUrl)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long id = await dbc.GetIdAsync<UserEntity>(u => u.Name == name);
                if (id>0)
                {
                    return -1;
                }
                UserEntity user = new UserEntity();
                user.Name = name;
                user.Salt = CommonHelper.GetCaptcha(4);
                user.Password = CommonHelper.GetMD5(password + user.Salt);
                user.NickName = string.IsNullOrEmpty(nickName) ? "无昵称" : nickName;
                user.HeadPic = string.IsNullOrEmpty(avatarUrl) ? "/images/headpic.png" : avatarUrl;
                dbc.Users.Add(user);
                await dbc.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<bool> UpdateInfoAsync(long id, string nickName, string headpic)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return false;
                }
                if (nickName != null)
                {
                    entity.NickName = nickName;
                }
                if (headpic != null)
                {
                    entity.HeadPic = headpic;
                }
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<long> DeleteAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                entity.IsDeleted = true;
                await dbc.SaveChangesAsync();
                return 1;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsEnabled = !entity.IsEnabled;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<long> ResetPasswordAsync(long id, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                entity.Password = CommonHelper.GetMD5(password + entity.Salt);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }  

        public async Task<long> CheckLoginAsync(string name, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Name == name);
                if (entity == null)
                {
                    return -1;
                }
                if (entity.Password != CommonHelper.GetMD5(password + entity.Salt))
                {
                    return -2;
                }
                if (entity.IsEnabled == false)
                {
                    return -3;
                }
                return entity.Id;
            }
        }

        public async Task<long> CheckUserMobileAsync(long id, string mobile)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetIdAsync<UserEntity>(u => u.Id==id && u.Mobile==mobile);
            }
        }

        public async Task<long> CheckUserNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetIdAsync<UserEntity>(u => u.Name == name);
            }
        }

        public bool CheckUserId(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long res = dbc.GetId<UserEntity>(u => u.Id == id);
                if (res <= 0)
                {
                    return false;
                }
                return true;
            }
        }

        public async Task<long> BindInfoAsync(long id, string mobile, string trueName, string wechatPayCode, string aliPayCode)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return -1;
                }
                if((await dbc.GetIdAsync<UserEntity>(u=>u.Mobile==mobile))>0)
                {
                    return -2;
                }
                user.Mobile = mobile;
                user.TrueName = trueName;
                user.WechatPayCode = wechatPayCode;
                user.AliPayCode = aliPayCode;
                await dbc.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<long> ResetBindInfoAsync(long id, string mobile, string trueName, string wechatPayCode, string aliPayCode)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id && u.Mobile==mobile);
                if (user == null)
                {
                    return -1;
                }
                user.Mobile = mobile;
                user.TrueName = trueName;
                user.WechatPayCode = wechatPayCode;
                user.AliPayCode = aliPayCode;
                await dbc.SaveChangesAsync();
                return user.Id;
            }
        }

        public string GetPayInfo(long userId, string payTypeName)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                if(payTypeName=="微信")
                {
                    return dbc.GetParameter<UserEntity>(u => u.Id == userId, u => u.WechatPayCode);
                }
                else
                {
                    return dbc.GetParameter<UserEntity>(u => u.Id == userId, u => u.AliPayCode);
                }
            }
        }

        public async Task<decimal> GetAmountByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetDecimalParameterAsync<UserEntity>(u=>u.Id==id,u=>u.Amount);
            }
        }

        public async Task<UserDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().AsNoTracking().SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<string> GetMobileByIdAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetParameterAsync<UserEntity>(u=>u.Id==id,u=>u.Mobile);
            }
        }

        public async Task<UserSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserSearchResult result = new UserSearchResult();
                var users = dbc.GetAll<UserEntity>().AsNoTracking();

                if (!string.IsNullOrEmpty(keyword))
                {
                    users = users.Where(a => a.Mobile.Contains(keyword) || a.Code.Contains(keyword) || a.NickName.Contains(keyword));
                }
                if (startTime != null)
                {
                    users = users.Where(a => a.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    users = users.Where(a => SqlFunctions.DateDiff("day", endTime, a.CreateTime) <= 0);
                }
                result.PageCount = (int)Math.Ceiling((await users.LongCountAsync()) * 1.0f / pageSize);
                var userResult = await users.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.Users = userResult.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
