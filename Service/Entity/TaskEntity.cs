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
        public string Code { get; set; }//任务编号
        public string Title { get; set; }//任务标题
        public decimal Bonus { get; set; }//任务佣金
        public string Condition { get; set; }//完成条件
        public string Explain { get; set; }//任务简介说明
        public string Content { get; set; }//任务内容
        public string Url { get; set; }//页面地址
        public DateTime StartTime { get; set; } = DateTime.Now;//任务开始时间
        public DateTime EndTime { get; set; }//任务结束时间
        public string Publisher { get; set; }//发布人
        public bool IsEnabled { get; set; } = true;
    }
}
