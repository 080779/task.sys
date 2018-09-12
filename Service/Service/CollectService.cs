using IMS.Common;
using IMS.DTO;
using IMS.IService;
using IMS.Service.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Service.Service
{
    public class CollectService : ICollectService
    {
        public async Task<long> CollectAsync(long userId, long taskId, bool isCollect)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                if(isCollect)
                {
                    if ((await dbc.GetIdAsync<CollectEntity>(c => c.UserId == userId && c.TaskId == taskId)) > 0)
                    {
                        return -1;
                    }
                    CollectEntity collect = new CollectEntity();
                    collect.TaskId = taskId;
                    collect.UserId = userId;
                    dbc.Collects.Add(collect);
                    await dbc.SaveChangesAsync();
                    return collect.Id;
                }
                else
                {
                    if ((await dbc.GetIdAsync<CollectEntity>(c => c.UserId == userId && c.TaskId == taskId)) <= 0)
                    {
                        return -2;
                    }
                    CollectEntity collect = await dbc.GetAll<CollectEntity>().SingleOrDefaultAsync(c=>c.UserId==userId && c.TaskId==taskId);
                    dbc.Collects.Remove(collect);
                    await dbc.SaveChangesAsync();
                    return 1;
                }
            }
        }
    }
}
