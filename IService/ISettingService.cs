using IMS.DTO;
using System;
using System.Threading.Tasks;

namespace IMS.IService
{
    /// <summary>
    /// 设置管理接口
    /// </summary>
    public interface ISettingService : IServiceSupport
    {
        Task<bool> UpdateAsync(long id, string parm);
        Task<bool> UpdateAsync(params SettingDTO[] parms);
        Task<string> GetParmByNameAsync(string name);
        Task<SettingDTO> GetModelAsync(long id);
        Task<SettingDTO> GetModelByNameAsync(string name);
    }
}
