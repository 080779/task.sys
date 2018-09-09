using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Service.Entity
{
    /// <summary>
    /// 任务收藏实体类
    /// </summary>
    public class CollectEntity : BaseEntity
    {
        public long TaskId { get; set; }
        public virtual TaskEntity Task { get; set; }
        public long UserId { get; set; }
        public virtual UserEntity User { get; set; }
    }
}
