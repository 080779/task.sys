using IMS.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.IService
{
    public interface IForwardService : IServiceSupport
    {
        Task<long> AcceptAsync(long taskId, long userId);
        Task<long> ForwardAsync(long id,string imgUrl);
        Task<long> ConfirmAsync(long id,bool auditState);
        Task<bool> DelAsync(long id, long userId);
        Task<string> GetStateNameAsync(long userId,long taskId);
        Task<long> GetUserForwardStatisticalAsync(long userId,DateTime? dateTime);
        Task<ForwardStatisticalResult> GetDayAsync(DateTime? dateTime);
        Task<ForwardStatisticalResult> GetMonthAsync(DateTime? dateTime);
        Task<ForwardSearchResult> GetModelListAsync(string keyword,int pageIndex,int pageSize);
    }
    /// <summary>
    /// 转发统计结果类
    /// </summary>
    public class ForwardStatisticalResult
    {
        public long TotalCount { get; set; }//转发数量
        public decimal TotalBonus { get; set; }//转发佣金
    }
    public class ForwardSearchResult
    {
        public ForwardDTO[] Forwards { get; set; }
        public long PageCount { get; set; }
    }
}
