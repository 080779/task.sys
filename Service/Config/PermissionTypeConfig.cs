using IMS.Service.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IMS.Service.Config
{
    class PermissionTypeConfig:EntityTypeConfiguration<PermissionTypeEntity>
    {
        public PermissionTypeConfig()
        {
            ToTable("tb_permissionTypes");
            Property(p => p.Name).HasMaxLength(30).IsRequired();
            Property(p => p.Description).HasMaxLength(100);
        }
    }
}
