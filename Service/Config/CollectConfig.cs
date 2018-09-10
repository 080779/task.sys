using IMS.Service.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IMS.Service.Config
{
    class CollectConfig : EntityTypeConfiguration<CollectEntity>
    {
        public CollectConfig()
        {
            ToTable("tb_collects");
            HasRequired(g => g.User).WithMany().HasForeignKey(g => g.UserId).WillCascadeOnDelete(false);
            HasRequired(g => g.Task).WithMany().HasForeignKey(g => g.TaskId).WillCascadeOnDelete(false);
        }
    }
}
