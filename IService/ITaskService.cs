﻿using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.IService
{
    public interface ITaskService : IServiceSupport
    {
        Task<long> AddAsync(string title,decimal bonus,string condition,string explain,string content,DateTime startTime,DateTime endTime);
        Task<bool> EditAsync(long id, string title, decimal bonus, string condition, string explain, string content, DateTime startTime, DateTime endTime);
        Task<bool> DelAsync(long id);
        Task<TaskSearchResult> GetModelListAsync(long? userId, int? within, int pageIndex, int pageSize);
        Task<TaskSearchResult> GetModelListAsync(long? userId, string keyword,DateTime? startTime,DateTime? endTime,int pageIndex,int pageSize);
    }
    public class TaskSearchResult
    {
        public TaskDTO[] Tasks { get; set; }
        public long PageCount { get; set; }
    }
}
