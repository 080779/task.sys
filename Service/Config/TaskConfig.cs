using IMS.Service.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IMS.Service.Config
{
    class TaskConfig : EntityTypeConfiguration<TaskEntity>
    {
        public TaskConfig()
        {
            ToTable("tb_tasks");

            Property(p => p.Code).HasMaxLength(100).IsRequired();
            Property(p => p.Title).HasMaxLength(100).IsRequired();
            Property(p => p.Url).HasMaxLength(100);
            Property(p => p.Condition).HasMaxLength(256).IsRequired();
            Property(p => p.Explain).HasMaxLength(256).IsRequired();
            Property(p => p.Content).HasMaxLength(4000);
            Property(p => p.Publisher).HasMaxLength(100);
        }
    }
}
