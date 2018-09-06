using IMS.Service.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IMS.Service.Config
{
    class SettingTypeConfig : EntityTypeConfiguration<SettingTypeEntity>
    {
        public SettingTypeConfig()
        {
            ToTable("tb_settingTypes");
            Property(s => s.Name).HasMaxLength(50).IsRequired();
        }
    }
}
