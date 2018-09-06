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
        private static ILog log = LogManager.GetLogger(typeof(UserService));
        public UserDTO ToDTO(UserEntity entity)
        {
            UserDTO dto = new UserDTO();
            dto.Amount = entity.Amount;
            dto.Code = entity.Code;
            dto.CreateTime = entity.CreateTime;
            dto.Description = entity.Description;
            dto.ErrorCount = entity.ErrorCount;
            dto.ErrorTime = entity.ErrorTime;
            dto.Id = entity.Id;
            dto.IsEnabled = entity.IsEnabled;
            dto.LevelId = entity.LevelId;
            dto.LevelName = entity.Level.Name;
            dto.Mobile = entity.Mobile;
            dto.NickName = entity.NickName;
            dto.BuyAmount = entity.BuyAmount;
            dto.IsReturned = entity.IsReturned;
            dto.IsUpgraded = entity.IsUpgraded;
            dto.BonusAmount = entity.BonusAmount;
            //dto.Recommender = entity.Recommend.RecommendMobile;
            dto.HeadPic = entity.HeadPic;
            dto.ShareCode = entity.ShareCode;
            dto.FrozenAmount = entity.FrozenAmount;
            return dto;
        }

        public async Task<long> AddAsync(string mobile, string password, string tradePassword, long levelTypeId, string recommendMobile,string nickName,string avatarUrl)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Mobile == mobile);
                if (entity != null)
                {
                    return -1;
                }
                UserEntity user = new UserEntity();
                user.LevelId = levelTypeId;
                user.Mobile = mobile;
                user.Salt = CommonHelper.GetCaptcha(4);
                user.Password = CommonHelper.GetMD5(password + user.Salt);
                user.TradePassword = CommonHelper.GetMD5(tradePassword + user.Salt);
                user.NickName = string.IsNullOrEmpty(nickName) ? "无昵称" : nickName;
                user.HeadPic = string.IsNullOrEmpty(avatarUrl) ? "/images/headpic.png" : avatarUrl;
                dbc.Users.Add(user);
                await dbc.SaveChangesAsync();
                return user.Id;
            }
        }

        public async Task<bool> AddAmountAsync(string mobile, decimal amount)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity user= await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Mobile == mobile);
                if(user==null)
                {
                    return false;
                }
                user.Amount = user.Amount + amount;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateInfoAsync(long id, string nickName, string headpic)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Id == id);
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

        public async Task<bool> UpdateShareCodeAsync(long id, string codeUrl)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.ShareCode = codeUrl;
                await dbc.SaveChangesAsync();
                return true;
            }
        }
        public async Task<long> DeleteAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                if(entity.Mobile=="15615615616")
                {
                    return -2;
                }
                await dbc.SaveChangesAsync();
                return 1;
            }
        }

        public async Task<bool> FrozenAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.IsEnabled = !entity.IsEnabled;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<long> ResetPasswordAsync(long id, string password, string newPassword)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                if (entity.Password != CommonHelper.GetMD5(password + entity.Salt))
                {
                    return -2;
                }
                entity.Password = CommonHelper.GetMD5(newPassword + entity.Salt);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> ResetPasswordAsync(long id, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return -1;
                }
                entity.Password = CommonHelper.GetMD5(password + entity.Salt);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> ResetPasswordAsync(string mobile, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Mobile == mobile);
                if (entity == null)
                {
                    return -1;
                }
                entity.Password = CommonHelper.GetMD5(password + entity.Salt);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }

        public async Task<long> ResetTradePasswordAsync(string mobile, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Mobile == mobile);
                if (entity == null)
                {
                    return -1;
                }
                entity.TradePassword = CommonHelper.GetMD5(password + entity.Salt);
                await dbc.SaveChangesAsync();
                return entity.Id;
            }
        }
    

        public async Task<long> UserCheck(string mobile)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long id = await dbc.GetIdAsync<UserEntity>(u => u.Mobile == mobile);
                if (id == 0)
                {
                    return -1;
                }
                return id;
            }
        }

        public async Task<long> CheckLoginAsync(string mobile, string password)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Mobile == mobile);
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

        public async Task<long> CheckTradePasswordAsync(long id, string tradePassword)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == id);
                if(user==null)
                {
                    return -1;
                }
                if (user.TradePassword != CommonHelper.GetMD5(tradePassword + user.Salt))
                {
                    return -2;
                }
                return 1;
            }
        }

        public bool CheckUserId(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long res = dbc.GetId<UserEntity>(u => u.Id == id);
                if (res == 0)
                {
                    return false;
                }
                return true;
            }
        }


        public async Task<CalcAmountResult> CalcCount()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CalcAmountResult res = new CalcAmountResult();
                var users = dbc.GetAll<UserEntity>().AsNoTracking().Where(u => u.IsNull == false);
                var takeCash = dbc.GetAll<TakeCashEntity>().AsNoTracking().Where(t => t.State.Name == "已结款");
                res.TotalAmount = users.Any() ? await users.SumAsync(u => u.Amount) : 0;
                res.TotalTakeCash = takeCash.Any() ? await takeCash.SumAsync(u => u.Amount) : 0;
                res.TotalBuyAmount = users.Any() ? await users.SumAsync(u => u.BuyAmount) : 0;
                return res;
            }
        }

        public async Task<UserDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity entity = await dbc.GetAll<UserEntity>().AsNoTracking().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Id == id);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }

        public async Task<string> GetMobileById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                string mobile = await dbc.GetParameterAsync<UserEntity>(u => u.Id == id, u => u.Mobile);
                if(mobile==null)
                {
                    return "";
                }
                return mobile;
            }
        }

        public async Task<UserDTO> GetModelByMobileAsync(string mobile)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserSearchResult result = new UserSearchResult();
                var user = await dbc.GetAll<UserEntity>().AsNoTracking().Where(u => u.IsNull == false).SingleOrDefaultAsync(u => u.Mobile == mobile);
                if (user == null)
                {
                    return null;
                }
                return ToDTO(user);
            }
        }

        public async Task<UserSearchResult> GetModelListAsync(long? levelId, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserSearchResult result = new UserSearchResult();
                var users = dbc.GetAll<UserEntity>().AsNoTracking().Where(u => u.IsNull == false);

                if (levelId != null)
                {
                    users = users.Where(a => a.LevelId == levelId);
                }
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
