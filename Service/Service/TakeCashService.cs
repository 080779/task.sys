using IMS.DTO;
using IMS.IService;
using IMS.Service.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Service.Service
{
    public class TakeCashService : ITakeCashService
    {
        public TakeCashDTO ToDTO(TakeCashEntity entity)
        {
            TakeCashDTO dto = new TakeCashDTO();
            dto.Amount = entity.Amount;
            dto.CreateTime = entity.CreateTime;
            dto.Description = entity.Description;
            dto.Id = entity.Id;
            dto.StateId = entity.StateId;
            dto.StateName = entity.State.Name;
            dto.TypeId = entity.TypeId;
            dto.TypeName = entity.Type.Name;
            //dto.PayCode = payCode;
            //dto.BankAccount = bankAccount;
            dto.Name = entity.User.Name;
            dto.Mobile = entity.User.Mobile;
            dto.Code = entity.User.Code;
            dto.AdminMobile = entity.AdminMobile;
            dto.UserId = entity.UserId;
            return dto;
        }

        public async Task<long> AddAsync(long userId, long payTypeId, decimal amount, string descripton)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    return -1;
                }
                if (user.Amount<amount)
                {
                    return -2;
                }
                if (string.IsNullOrEmpty(user.Mobile))
                {
                    return -4;
                }

                TakeCashEntity takeCash = new TakeCashEntity();
                takeCash.UserId = userId;
                takeCash.TypeId = payTypeId;
                var stateId = await dbc.GetIdAsync<IdNameEntity>(i => i.Name == "未结款");
                if (stateId == 0)
                {
                    return -3;
                }
                takeCash.StateId = stateId;
                takeCash.Amount = amount;
                takeCash.Description = descripton;
                dbc.TakeCashes.Add(takeCash);
                await dbc.SaveChangesAsync();
                user.Amount = user.Amount - takeCash.Amount;
                JournalEntity journal = new JournalEntity();
                journal.OutAmount = takeCash.Amount;
                journal.JournalTypeId = await dbc.GetIdAsync<IdNameEntity>(i => i.Name == "余额提现中");
                journal.Remark = "余额提现";
                journal.UserId = takeCash.UserId;
                journal.BalanceAmount = user.Amount;
                journal.Journal01 = takeCash.Id;
                dbc.Journals.Add(journal);
                await dbc.SaveChangesAsync();
                return takeCash.Id;
            }
        }

        public async Task<long> Confirm(long id, long adminId, bool isSuccess)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TakeCashEntity takeCash = await dbc.GetAll<TakeCashEntity>().SingleOrDefaultAsync(t => t.Id == id);
                if (takeCash == null)
                {
                    return -1;
                }
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == takeCash.UserId);
                if (user == null)
                {
                    return -2;
                }
                if (isSuccess == false)
                {                    
                    takeCash.StateId = await dbc.GetIdAsync<IdNameEntity>(i => i.Name == "已取消");
                    takeCash.AdminMobile = await dbc.GetParameterAsync<AdminEntity>(a => a.Id == adminId, a => a.Mobile);
                    JournalEntity journal = await dbc.GetAll<JournalEntity>().SingleOrDefaultAsync(j=>j.Journal01==takeCash.Id && j.JournalType.Name== "余额提现中");
                    user.Amount = user.Amount + journal.OutAmount.Value;
                    await dbc.SaveChangesAsync();
                    return -4;
                }                
                else
                {
                    takeCash.StateId = await dbc.GetIdAsync<IdNameEntity>(i => i.Name == "已结款");
                    takeCash.AdminMobile = await dbc.GetParameterAsync<AdminEntity>(a => a.Id == adminId, a => a.Mobile);
                    JournalEntity journal = await dbc.GetAll<JournalEntity>().SingleOrDefaultAsync(j=>j.Journal01==takeCash.Id && j.JournalType.Name== "余额提现中");
                    journal.JournalTypeId = await dbc.GetIdAsync<IdNameEntity>(i=>i.Name== "余额提现");
                    user.TakeCashAmount = user.TakeCashAmount + journal.OutAmount.Value;
                    await dbc.SaveChangesAsync();
                    return takeCash.Id;
                }
            }
        }

        public async Task<TakeCashSearchResult> GetModelListAsync(long? userId, long? stateId, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TakeCashSearchResult result = new TakeCashSearchResult();
                var entities = dbc.GetAll<TakeCashEntity>();
                if (userId != null)
                {
                    entities = entities.Where(a => a.UserId == userId);
                }
                if (stateId != null)
                {
                    entities = entities.Where(a => a.StateId == stateId);
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(g => g.User.Mobile.Contains(keyword) || g.User.NickName.Contains(keyword));
                }
                if (startTime != null)
                {
                    entities = entities.Where(a => a.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    entities = entities.Where(a => SqlFunctions.DateDiff("day", endTime, a.CreateTime) <= 0);
                }
                result.PageCount = (int)Math.Ceiling((await entities.LongCountAsync()) * 1.0f / pageSize);
                var takeCashResult = await entities.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.TakeCashes = takeCashResult.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
