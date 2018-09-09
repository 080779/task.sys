using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DTO
{
    public class UserDTO : BaseDTO
    {
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string TrueName { get; set; }
        public string NickName { get; set; }
        public string HeadPic { get; set; }
        public decimal Amount { get; set; }
        public long? LevelId { get; set; }
        public string WechatPayCode { get; set; }
        public string AliPayCode { get; set; }
        public string AccountHolder { get; set; }//银行账户持有人
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public bool IsEnabled { get; set; }
    }
}
