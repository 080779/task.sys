﻿using IMS.Common;
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
        public TaskDTO ToDTO(TaskEntity entity)
        {
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
            dto.Url = entity.Url;
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

        public async Task<long> AddAsync(string title, decimal bonus, string condition, string explain, string content, DateTime startTime, DateTime endTime)
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
                task.StartTime = startTime;
                task.EndTime = endTime;
                dbc.Tasks.Add(task);
                await dbc.SaveChangesAsync();
                return task.Id;
            }
        }

        public async Task<bool> EditAsync(long id, string title, decimal bonus, string condition, string explain, string content, DateTime startTime, DateTime endTime)
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
                task.StartTime = startTime;
                task.EndTime = endTime;
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

        public async Task<TaskSearchResult> GetModelListAsync(int? within, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskSearchResult result = new TaskSearchResult();
                var entities = dbc.GetAll<TaskEntity>();                
                if(within!=null)
                {
                    DateTime date = DateTime.Now.AddDays(-within.Value);
                    entities = entities.Where(t => t.CreateTime >= date);
                }
                result.PageCount = (int)Math.Ceiling((await entities.LongCountAsync()) * 1.0f / pageSize);
                var taskResult = await entities.OrderByDescending(a => a.CreateTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
                result.Tasks = taskResult.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        }

        public async Task<TaskSearchResult> GetModelListAsync(string keyword, DateTime? startTime, DateTime? endTime, int pageIndex, int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TaskSearchResult result = new TaskSearchResult();
                var entities = dbc.GetAll<TaskEntity>();
                if (!string.IsNullOrEmpty(keyword))
                {
                    entities = entities.Where(g => g.Title.Contains(keyword));
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
                result.Tasks = taskResult.Select(a => ToDTO(a)).ToArray();
                return result;
            }
        } 
    }
}
