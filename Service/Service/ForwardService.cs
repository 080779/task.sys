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
        public async Task<long> ForwardAsync(long taskId, long userId, string imgUrl)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                ForwardEntity forward = new ForwardEntity();
                forward.TaskId = taskId;
                forward.UserId = userId;
                forward.ImgUrl = imgUrl;
                long stateId= await dbc.GetIdAsync<ForwardStateEntity>(f => f.Name == "审核中"); 
                if(stateId<=0)
                {
                    return -1;
                }
                forward.StateId = stateId;
                dbc.Forwards.Add(forward);
                await dbc.SaveChangesAsync();
                return forward.Id;
            }
        }

        public async Task<long> Confirm(long id, bool auditState)
        {
            using (MyDbContext dbc = new MyDbContext())
            {                
                ForwardEntity forward = await dbc.GetAll<ForwardEntity>().Include(f=>f.Task).SingleOrDefaultAsync(f => f.Id == id);
                if(forward==null)
                {
                    return -1;
                }
                long stateId = await dbc.GetIdAsync<ForwardStateEntity>(f => f.Name == "转发失败");                
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
                stateId= await dbc.GetIdAsync<ForwardStateEntity>(f => f.Name == "转发成功");
                forward.StateId = stateId;
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u=>u.Id==forward.UserId);
                if(user==null)
                {
                    return -4;
                }
                decimal bonus= forward.Task.Bonus;
                user.Amount = user.Amount + bonus;
                long journalTypeId= await dbc.GetIdAsync<ForwardStateEntity>(f => f.Name == "任务转发");
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
    }
}
