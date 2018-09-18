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
    public class SettingService : ISettingService
    {
        private SettingDTO ToDTO(SettingEntity entity)
        {
            SettingDTO dto = new SettingDTO();
            dto.Id = entity.Id;
            dto.Parm = entity.Parm;
            dto.Name = entity.Name;
            dto.TypeId = entity.TypeId;
            return dto;
        }


        public async Task<string> GetParmByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                return await dbc.GetParameterAsync<SettingEntity>(s=>s.Name==name,s=>s.Parm);                 
            }
        }

        public async Task<SettingDTO> GetModelAsync(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                SettingEntity entity = await dbc.GetAll<SettingEntity>().Include(s => s.Type).AsNoTracking().SingleOrDefaultAsync(g => g.Id == id);
                if (entity == null)
                {
                    return null;
                }                
                return ToDTO(entity);
            }
        }

        public async Task<SettingDTO> GetModelByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                SettingEntity entity = await dbc.GetAll<SettingEntity>().Include(s => s.Type).AsNoTracking().SingleOrDefaultAsync(g => g.Name == name);
                if (entity == null)
                {
                    return null;
                }
                return ToDTO(entity);
            }
        }
        
        public async Task<SettingDTO[]> GetModelListByDescAsync(string desc)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                List<SettingEntity> settings = await dbc.GetAll<SettingEntity>().Include(s => s.Type).AsNoTracking().Where(g => g.Description == desc).ToListAsync();
                return settings.Select(s => ToDTO(s)).ToArray();
            }
        }

        public async Task<bool> UpdateAsync(long id, string parm)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                SettingEntity entity = await dbc.GetAll<SettingEntity>().SingleOrDefaultAsync(g => g.Id == id);
                if (entity == null)
                {
                    return false;
                }
                entity.Parm = parm;
                await dbc.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> UpdateAsync(params SettingDTO[] settings)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                foreach(SettingDTO setting in settings)
                {
                    SettingEntity entity = await dbc.GetAll<SettingEntity>().SingleOrDefaultAsync(g => g.Id == setting.Id);
                    if (entity == null)
                    {
                        return false;
                    }
                    entity.Parm = setting.Parm.ToString();
                }
                await dbc.SaveChangesAsync();
                return true;
            }
        }
    }
}
