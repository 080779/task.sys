using IMS.Service.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IMS.Service.Config
{
    class ForwardStateConfig : EntityTypeConfiguration<ForwardStateEntity>
    {
        public ForwardStateConfig()
        {
            ToTable("tb_forwardStates");
            Property(f => f.Name).HasMaxLength(50).IsRequired();
        }
    }
}
