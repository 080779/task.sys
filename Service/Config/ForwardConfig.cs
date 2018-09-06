using IMS.Service.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IMS.Service.Config
{
    class ForwardConfig : EntityTypeConfiguration<ForwardEntity>
    {
        public ForwardConfig()
        {
            ToTable("tb_forwards");
            HasRequired(g => g.User).WithMany().HasForeignKey(g => g.UserId).WillCascadeOnDelete(false);
            HasRequired(g => g.Task).WithMany().HasForeignKey(g => g.TaskId).WillCascadeOnDelete(false);
            HasRequired(g => g.State).WithMany().HasForeignKey(g => g.StateId).WillCascadeOnDelete(false);
            Property(g => g.ImgUrl).HasMaxLength(256);
        }
    }
}
