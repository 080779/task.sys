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
    public class TaskService : ITaskService
    {
        public TaskDTO ToDTO(TaskEntity entity,long collectId)
        {
            string domain = System.Configuration.ConfigurationManager.AppSettings["DoMain"];
            TaskDTO dto = new TaskDTO();
            dto.Bonus = entity.Bonus;
            dto.Code = entity.Code;
            dto.Condition = entity.Condition;
            dto.Content = entity.Content;
            dto.CreateTime = entity.CreateTime;
            dto.EndTime = entity.EndTime;
            dto.Explain = entity.Explain;
            dto.Id = entity.Id;
            dto.IsEnabled = entity.IsEnabled;
            dto.Publisher = entity.Publisher;
            dto.StartTime = entity.StartTime;
            dto.Title = entity.Title;
            dto.Url = domain + entity.Url;
            dto.IsCollect = collectId <= 0 ? false : true;
            return dto;
        }
        
        public async Task<long> Confirm(long id,long adminId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TakeCashEntity takeCash = await dbc.GetAll<TakeCashEntity>().SingleOrDefaultAsync(t=>t.Id==id);
                if(takeCash==null)
                {
                    return -1;
                }
                UserEntity user = await dbc.GetAll<UserEntity>().SingleOrDefaultAsync(u => u.Id == takeCash.UserId);
                if(user==null)
                {
                    return -2;
                }
                if(takeCash.Amount>user.Amount)
                {
                    return -3;
                }
                user.Amount = user.Amount - takeCash.Amount;
                takeCash.StateId = (await dbc.GetAll<IdNameEntity>().SingleOrDefaultAsync(i => i.Name == "已结款")).Id;
                takeCash.AdminMobile = (await dbc.GetAll<AdminEntity>().SingleOrDefaultAsync(a => a.Id == adminId)).Mobile;
                JournalEntity journal = new JournalEntity();
                journal.OutAmount = takeCash.Amount;
                journal.JournalTypeId = (await dbc.GetAll<IdNameEntity>().SingleOrDefaultAsync(i => i.Name == "余额提现")).Id;
                journal.Remark = "余额提现";
                journal.UserId = takeCash.UserId;
                journal.BalanceAmount = user.Amount;
                dbc.Journals.Add(journal);
                await dbc.SaveChangesAsync();
                return takeCash.Id;
            }
        }

        public async Task<long> AddAsync(string title, decimal bonus, string condition, string explain, string content, DateTime endTime, string publisher)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskEntity task = new TaskEntity();
                task.Code = CommonHelper.GetRandom2();
                task.Title = title;
                task.Bonus = bonus;
                task.Condition = condition;
                task.Explain = explain;
                task.Content = content;
                //task.StartTime = startTime;
                task.EndTime = endTime;
                task.Publisher = publisher;
                dbc.Tasks.Add(task);
                await dbc.SaveChangesAsync();
                task.Url = "/task/info?id=" + task.Id;
                task.IsEnabled = endTime > DateTime.Now;
                await dbc.SaveChangesAsync();
                return task.Id;
            }
        }

        public async Task<bool> EditAsync(long id, string title, decimal bonus, string condition, string explain, string content, DateTime endTime)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskEntity task = await dbc.GetAll<TaskEntity>().SingleOrDefaultAsync(t=>t.Id==id);
                if(task==null)
                {
                    return false;
                }
                task.Title = title;
                task.Bonus = bonus;
                task.Condition = condition;
                task.Explain = explain;
                task.Content = content;
                //task.StartTime = startTime;
                task.EndTime = endTime;
                task.IsEnabled = endTime > DateTime.Now;
                if (string.IsNullOrEmpty(task.Url))
                {
                    task.Url= "/task/info?id=" + task.Id;
                }
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskEntity task = await dbc.GetAll<TaskEntity>().SingleOrDefaultAsync(t => t.Id == id);
                if (task == null)
                {
                    return false;
                }
                task.IsDeleted = true;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<TaskDTO> GetModelAsync(long id,long userId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskEntity task = await dbc.GetAll<TaskEntity>().SingleOrDefaultAsync(t=>t.Id==id);
                if(task==null)
                {
                    return null;
                }
                task.IsEnabled = task.EndTime > DateTime.Now;
                await dbc.SaveChangesAsync();
                return ToDTO(task, dbc.GetId<CollectEntity>(c => c.UserId == userId && c.TaskId == id));
            }
        }

        public async Task<TaskSearchResult> GetModelListCollectAsync(long? userId, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskSearchResult result = new TaskSearchResult();
                IQueryable<TaskEntity> tasks = dbc.GetAll<CollectEntity>().Where(c => c.UserId == userId).Select(c => c.Task).Where(t => t.IsDeleted == false);
                result.PageCount = (int)Math.Ceiling((await tasks.LongCountAsync()) * 1.0f / pageSize);
                var taskResult = await tasks.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                DateTime now = DateTime.Now;
                foreach (var task in taskResult)
                {
                    task.IsEnabled = task.EndTime > now;
                }
                await dbc.SaveChangesAsync();
                result.Tasks = taskResult.Select(a => ToDTO(a, dbc.GetId<CollectEntity>(c => c.UserId == userId && c.TaskId == a.Id))).ToArray();
                return result;
            }
        }


        public async Task<TaskSearchResult> GetModelListForwardAsync(long? userId, long? forwardStateId, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskSearchResult result = new TaskSearchResult();
                IQueryable<ForwardEntity> forwards = dbc.GetAll<ForwardEntity>().Where(c => c.UserId == userId);
                if(forwardStateId!=null)
                {
                    forwards = forwards.Where(f=>f.StateId==forwardStateId);
                }
                IQueryable<TaskEntity> tasks = forwards.Select(c => c.Task).Where(t => t.IsDeleted == false);
                result.PageCount = (int)Math.Ceiling((await tasks.LongCountAsync()) * 1.0f / pageSize);
                var taskResult = await tasks.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                DateTime now = DateTime.Now;
                foreach (var task in taskResult)
                {
                    task.IsEnabled = task.EndTime > now;
                }
                await dbc.SaveChangesAsync();
                result.Tasks = taskResult.Select(a => ToDTO(a, dbc.GetId<CollectEntity>(c => c.UserId == userId && c.TaskId == a.Id))).ToArray();
                return result;
            }
        }

        public async Task<TaskSearchResult> GetModelListForwardingAsync(long? userId, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskSearchResult result = new TaskSearchResult();
                IQueryable<ForwardEntity> forwards = dbc.GetAll<ForwardEntity>().Where(c => c.UserId == userId).Where(c=>c.State.Name== "已接受" || c.State.Name== "审核中");
                IQueryable<TaskEntity> tasks = forwards.Select(c => c.Task).Where(t => t.IsDeleted == false);
                result.PageCount = (int)Math.Ceiling((await tasks.LongCountAsync()) * 1.0f / pageSize);
                var taskResult = await tasks.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                DateTime now = DateTime.Now;
                foreach (var task in taskResult)
                {
                    task.IsEnabled = task.EndTime > now;
                }
                await dbc.SaveChangesAsync();
                result.Tasks = taskResult.Select(a => ToDTO(a, dbc.GetId<CollectEntity>(c => c.UserId == userId && c.TaskId == a.Id))).ToArray();
                return result;
            }
        }

        public async Task<TaskSearchResult> GetModelListAsync(long? userId, int? within, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskSearchResult result = new TaskSearchResult();
                var entities = dbc.GetAll<TaskEntity>().Where(t => t.IsEnabled == true);                
                if(within!=null)
                {
                    DateTime date = DateTime.Now.AddDays(-within.Value);
                    entities = entities.Where(t => t.CreateTime >= date);
                }
                result.PageCount = (int)Math.Ceiling((await entities.LongCountAsync()) * 1.0f / pageSize);               
                var taskResult = await entities.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                DateTime now = DateTime.Now;
                foreach (var task in taskResult)
                {
                    task.IsEnabled = task.EndTime > now;
                }
                await dbc.SaveChangesAsync();
                if (userId==null)
                {
                    result.Tasks = taskResult.Select(a => ToDTO(a,0)).ToArray();
                }
                else
                {
                    result.Tasks = taskResult.Select(a => ToDTO(a, dbc.GetId<CollectEntity>(c => c.UserId == userId && c.TaskId == a.Id))).ToArray();
                }               
                return result;
            }
        }

        public async Task<TaskSearchResult> GetModelListAsync(bool isAdmin, long? userId, string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskSearchResult result = new TaskSearchResult();
                var entities = dbc.GetAll<TaskEntity>();
                if(!isAdmin)
                {
                    entities = entities.Where(t=>t.IsEnabled==true);
                }
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(g => g.Title.Contains(keyword) || g.Condition.Contains(keyword) || g.Explain.Contains(keyword) || g.Content.Contains(keyword));
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
                var taskResult = await entities.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                DateTime now = DateTime.Now;
                foreach (var task in taskResult)
                {
                    task.IsEnabled = task.EndTime > now;
                }
                await dbc.SaveChangesAsync();
                if (userId == null)
                {
                    result.Tasks = taskResult.Select(a => ToDTO(a, 0)).ToArray();
                }
                else
                {
                    result.Tasks = taskResult.Select(a => ToDTO(a, dbc.GetId<CollectEntity>(c => c.UserId == userId && c.TaskId == a.Id))).ToArray();
                }
                return result;
            }
        }
    }
}
