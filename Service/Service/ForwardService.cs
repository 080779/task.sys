using IMS.Common;
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
    public class ForwardService : IForwardService
    {
        public ForwardDTO ToDTO(ForwardEntity entity)
        {
            ForwardDTO dto = new ForwardDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.ImgUrl = entity.ImgUrl;
            dto.StateId = entity.StateId;
            dto.StateName = entity.State.Name;
            dto.TaskId = entity.TaskId;
            dto.TaskTitle = entity.Task.Title;
            dto.UserId = entity.UserId;
            dto.UserName = entity.User.Name;
            dto.Amount = entity.User.Amount;
            dto.TakeCashAmount = entity.User.TakeCashAmount;
            dto.BonusAmount = entity.User.BonusAmount;
            return dto;
        }

        public async Task<long> AcceptAsync(long taskId, long userId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskEntity task = await dbc.GetAll<TaskEntity>().SingleOrDefaultAsync(t => t.Id == taskId);
                if (task == null)
                {
                    return -1;
                }
                if (task.IsEnabled == false)
                {
                    return -2;
                }

                if (string.IsNullOrEmpty(await dbc.GetParameterAsync<UserEntity>(u => u.Id == userId, u => u.Mobile)))
                {
                    return -3;
                }

                long stateId = await dbc.GetIdAsync<ForwardStateEntity>(f => f.Name == "已接受");
                if (stateId <= 0)
                {
                    return -4;
                }

                ForwardEntity forward = await dbc.GetAll<ForwardEntity>().Include(f=>f.State).SingleOrDefaultAsync(f=>f.UserId==userId && f.TaskId==taskId);
                if(forward==null)
                {
                    forward = new ForwardEntity();
                    forward.TaskId = taskId;
                    forward.UserId = userId;
                    forward.StateId = stateId;
                    dbc.Forwards.Add(forward);
                    await dbc.SaveChangesAsync();
                    return forward.Id;
                }
                if(forward.State.Name != "未通过审核")
                {
                    return -5;
                }
                forward.StateId = stateId;
                await dbc.SaveChangesAsync();               
                return forward.Id;
            }
        }

        public async Task<long> ForwardAsync(long taskId, long userId, string imgUrl)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ForwardEntity forward = await dbc.GetAll<ForwardEntity>().Include(f=>f.Task).SingleOrDefaultAsync(t => t.UserId == userId && t.TaskId==taskId);
                if (forward == null)
                {
                    return -1;
                }
                if(forward.Task.IsEnabled==false)
                {
                    return -2;
                }

                if(string.IsNullOrEmpty(await dbc.GetParameterAsync<UserEntity>(u => u.Id == userId, u => u.Mobile)))
                {
                    return -3;
                }

                long stateId = await dbc.GetIdAsync<ForwardStateEntity>(f => f.Name == "审核中");
                if (stateId <= 0)
                {
                    return -4;
                }
                forward.ImgUrl = imgUrl;
                forward.StateId = stateId;
                await dbc.SaveChangesAsync();
                return forward.Id;
            }
        }

        public async Task<long> ConfirmAsync(long id, bool auditState)
        {
            using (MyDbContext dbc = new MyDbContext())
            {                
                ForwardEntity forward = await dbc.GetAll<ForwardEntity>().Include(f=>f.Task).SingleOrDefaultAsync(f => f.Id == id);
                if(forward==null)
                {
                    return -1;
                }
                long stateId = await dbc.GetIdAsync<ForwardStateEntity>(f => f.Name == "未通过审核");                
                if (stateId <= 0)
                {
                    return -2;
                }
                if (auditState == false)
                {
                    forward.StateId = stateId;
                    await dbc.SaveChangesAsync();
                    return -3;
                }
                stateId= await dbc.GetIdAsync<ForwardStateEntity>(f => f.Name == "任务完成");
                forward.StateId = stateId;
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u=>u.Id==forward.UserId);
                if(user==null)
                {
                    return -4;
                }
                decimal bonus= forward.Task.Bonus;
                user.Amount = user.Amount + bonus;
                user.BonusAmount = user.BonusAmount + bonus;
                long journalTypeId= await dbc.GetIdAsync<IdNameEntity>(f => f.Name == "任务转发");
                if(journalTypeId <= 0)
                {
                    return -5;
                }
                JournalEntity journal = new JournalEntity();
                journal.BalanceAmount = user.Amount;
                journal.ForwardId = forward.Id;
                journal.TaskId = forward.TaskId;
                journal.InAmount = bonus;
                journal.JournalTypeId = journalTypeId;
                journal.Remark = "任务转发获得佣金";
                journal.UserId = user.Id;
                dbc.Journals.Add(journal);
                await dbc.SaveChangesAsync();
                return forward.Id;
            }
        }

        public async Task<bool> DelAsync(long id,long userId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ForwardEntity forward = await dbc.GetAll<ForwardEntity>().SingleOrDefaultAsync(f => f.TaskId == id && f.UserId==userId);
                if (forward == null)
                {
                    return false;
                }
                dbc.Forwards.Remove(forward);
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<string> GetStateNameAsync(long userId, long taskId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long id = await dbc.GetlongParameterAsync<ForwardEntity>(f => f.UserId == userId && f.TaskId == taskId, f => f.StateId);
                if(id==0)
                {
                    return null;
                }
                return await dbc.GetParameterAsync<ForwardStateEntity>(i=>i.Id==id,i=>i.Name);
            }
        }

        public async Task<long> GetUserForwardStatisticalAsync(long userId, DateTime? dateTime)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                if(dateTime==null)
                {
                    IQueryable<ForwardEntity> forwards = dbc.GetAll<ForwardEntity>().Where(f => f.UserId == userId);
                    return await forwards.LongCountAsync();
                }
                else
                {
                    IQueryable<ForwardEntity> forwards = dbc.GetAll<ForwardEntity>().Where(f => f.UserId == userId).Where(f => SqlFunctions.DateDiff("month", dateTime, f.CreateTime) == 0);
                    return await forwards.LongCountAsync();
                }
            }
        }

        public async Task<ForwardStatisticalResult> GetDayAsync(DateTime? dateTime)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ForwardStatisticalResult result = new ForwardStatisticalResult();
                IQueryable<ForwardEntity> forwards = dbc.GetAll<ForwardEntity>().Where(f => SqlFunctions.DateDiff("day", dateTime, f.CreateTime) == 0);
                if(!await forwards.AnyAsync())
                {
                    result.TotalBonus = 0;
                    result.TotalCount = 0;
                }
                else
                {
                    result.TotalBonus = await forwards.SumAsync(f => f.Task.Bonus);
                    result.TotalCount = await forwards.LongCountAsync();
                }
                return result;
            }
        }

        public async Task<ForwardStatisticalResult> GetMonthAsync(DateTime? dateTime)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ForwardStatisticalResult result = new ForwardStatisticalResult();
                IQueryable<ForwardEntity> forwards = dbc.GetAll<ForwardEntity>().Where(f => SqlFunctions.DateDiff("month", dateTime, f.CreateTime) == 0);
                if (!await forwards.AnyAsync())
                {
                    result.TotalBonus = 0;
                    result.TotalCount = 0;
                }
                else
                {
                    result.TotalBonus = await forwards.SumAsync(f => f.Task.Bonus);
                    result.TotalCount = await forwards.LongCountAsync();
                }
                return result;
            }
        }

        public async Task<ForwardSearchResult> GetModelListAsync(string keyword, long? stateId, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ForwardSearchResult result = new ForwardSearchResult();
                IQueryable<ForwardEntity> forwards = dbc.GetAll<ForwardEntity>();
                if(!string.IsNullOrEmpty(keyword))
                {
                    forwards = forwards.Where(f=>f.User.Name.Contains(keyword));
                }
                if(stateId!=null)
                {
                    forwards = forwards.Where(f=>f.StateId==stateId);
                }
                if (startTime != null)
                {
                    forwards = forwards.Where(f => f.CreateTime >= startTime);
                }
                if (endTime != null)
                {
                    forwards = forwards.Where(f => SqlFunctions.DateDiff("day", endTime, f.CreateTime) <= 0);
                }
                result.PageCount = (int)Math.Ceiling((await forwards.LongCountAsync()) * 1.0f / pageSize);
                var forwordResult = await forwards.Include(f=>f.User).Include(f=>f.Task).Include(f=>f.State).OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.Forwards = forwordResult.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }
    }
}
