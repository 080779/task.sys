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
        Task<long> ForwardAsync(long taskId,long userId,string imgUrl);
        Task<long> ConfirmAsync(long id,bool auditState);
        Task<long> GetUserForwardStatisticalAsync(long userId,DateTime? dateTime);
        Task<ForwardStatisticalResult> GetDayAsync(DateTime dateTime);
        Task<ForwardStatisticalResult> GetMonthAsync(DateTime dateTime);
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
