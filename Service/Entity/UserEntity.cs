using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Service.Entity
{
    /// <summary>
    /// 用户实体类
    /// </summary>
    public class UserEntity : BaseEntity
    {
        public string Mobile { get; set; } = string.Empty;
        public string Name { get; set; }
        public string Code { get; set; }
        public string TrueName { get; set; }
        public string NickName { get; set; }
        public string HeadPic { get; set; }
        public decimal Amount { get; set; } = 0;//账户金额
        public decimal BonusAmount { get; set; } = 0;//佣金总额
        public decimal TakeCashAmount { get; set; } = 0;//提现总额
        public long? LevelId { get; set; } = 0;
        //public virtual IdNameEntity Level { get; set; }
        public string Description { get; set; }
        public string Salt { get; set; } = string.Empty;
        public string Password { get; set; }
        public string TradePassword { get; set; }
        public string WechatPayCode { get; set; }
        public string AliPayCode { get; set; }
        public int ErrorCount { get; set; } = 0;
        public DateTime ErrorTime { get; set; } = DateTime.Now;
        public string AccountHolder { get; set; }//银行账户持有人
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public bool IsEnabled { get; set; } = true;
    }
}
