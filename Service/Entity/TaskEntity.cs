using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Service.Entity
{
    /// <summary>
    /// 任务实体类
    /// </summary>
    public class TaskEntity : BaseEntity
    {
        public string Name { get; set; }//任务名
        public string Explain { get; set; }//任务简介说明
        public string Content { get; set; }//任务内容
        public decimal Bonus { get; set; }//任务佣金
        public DateTime StartTime { get; set; }//任务开始时间
        public DateTime EndTime { get; set; }//任务结束时间
        public string Publisher { get; set; }//发布人
    }
}
