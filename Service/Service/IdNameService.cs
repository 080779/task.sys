using IMS.DTO;
using IMS.IService;
using IMS.Service.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Service.Service
{
    public class IdNameService : IIdNameService
    {
        private IdNameDTO ToDTO(IdNameEntity entity)
        {
            IdNameDTO dto = new IdNameDTO();
            dto.CreateTime = entity.CreateTime;
            dto.Id = entity.Id;
            dto.Name = entity.Name;
            dto.Description = entity.Description;
            dto.TypeName = entity.TypeName;
            return dto;
        }

        public async Task<bool> DelByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                IdNameEntity idName = await dbc.GetAll<IdNameEntity>().SingleOrDefaultAsync(i=>i.Name==name);
                if(idName==null)
                {
                    return false;
                }
                idName.IsDeleted = true;
                await dbc.SaveChangesAsync();
                return true;
            }            
        }

        public async Task<long> GetIdByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                long id = await dbc.GetIdAsync<IdNameEntity>(i => i.Name == name);
                if (id == 0)
                {
                    return -1;
                }
                return id;
            }
        }

        public async Task<IdNameDTO> GetByNameAsync(string name)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entitiy = await dbc.GetAll<IdNameEntity>().AsNoTracking().SingleOrDefaultAsync(i => i.Name == name);
                if (entitiy == null)
                {
                    return null;
                }
                return ToDTO(entitiy);
            }
        }

        public async Task<IdNameDTO[]> GetByTypeNameAsync(string typeName)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                var entities = dbc.GetAll<IdNameEntity>().AsNoTracking().Where(i => i.TypeName == typeName && i.IsNull == false);
                var result= await entities.OrderBy(i => i.Id).ToListAsync();
                return result.Select(i => ToDTO(i)).ToArray();
            }
        }
    }
}
