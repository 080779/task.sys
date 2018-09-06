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
        Task<long> Confirm(long id,bool auditState);
    }
}
