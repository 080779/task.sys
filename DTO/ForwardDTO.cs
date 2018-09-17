using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.DTO
{
    public class ForwardDTO : BaseDTO
    {
        public long TaskId { get; set; }
        public string TaskTitle { get; set; }
        public long UserId { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public decimal TakeCashAmount { get; set; }
        public decimal BonusAmount { get; set; }
        public long StateId { get; set; }
        public string StateName { get; set; }
        public string ImgUrl { get; set; }
    }
}
