using log4net;
using IMS.Service.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace IMS.Service
{
    public class MyDbContext:DbContext
    {
        //private static ILog log = LogManager.GetLogger(typeof(MyDbContext));
        public MyDbContext() : base("name=connStr") //“connStr”数据库连接字符串
        {
            //this.Database.Log = sql => log.DebugFormat("EF执行SQL：{0}", sql);//用log4NET记录数据操作日志
            //Database.SetInitializer<MyDbContext>(null);
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
        }
        public IQueryable<T> GetAll<T>() where T:BaseEntity
        {
            return this.Set<T>().Where(e => e.IsDeleted == false);
        }

        public long GetId<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(e => e.Id).SingleOrDefault();
        }

        public async Task<long> GetIdAsync<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(e => e.Id).SingleOrDefaultAsync();
        }

        public IQueryable<long> GetIds<T>(Expression<Func<T, bool>> expression) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(e => e.Id);
        }

        public string GetParameter<T>(Expression<Func<T, bool>> expression, Expression<Func<T, string>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<string> GetParameterAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, string>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }

        public long GetlongParameter<T>(Expression<Func<T, bool>> expression, Expression<Func<T, long>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<long> GetlongParameterAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, long>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }

        public decimal GetDecimalParameter<T>(Expression<Func<T, bool>> expression, Expression<Func<T, decimal>> parameterName) where T : BaseEntity
        {
            return this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(parameterName).SingleOrDefault();
        }

        public async Task<decimal> GetDecimalParameterAsync<T>(Expression<Func<T, bool>> expression, Expression<Func<T, decimal>> parameterName) where T : BaseEntity
        {
            return await this.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false).Where(expression).Select(parameterName).SingleOrDefaultAsync();
        }

        public DbSet<UserEntity> Users { get; set; }
        public DbSet<NavBarEntity> NavBars { get; set; }
        public DbSet<AdminEntity> Admins { get; set; }
        public DbSet<AdminLogEntity> AdminLogs { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<PermissionTypeEntity> PermissionTypes { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<TakeCashEntity> TakeCashes { get; set; }
        public DbSet<IdNameEntity> IdNames { get; set; }
        public DbSet<JournalEntity> Journals { get; set; }
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<ForwardEntity> Forwards { get; set; }
        public DbSet<ForwardStateEntity> ForwardStates { get; set; }
        public DbSet<CollectEntity> Collects { get; set; }
    }
}
