﻿using IMS.Service.Entity;
using System.Data.Entity.ModelConfiguration;

namespace IMS.Service.Config
{
    class TaskConfig : EntityTypeConfiguration<TaskEntity>
    {
        public TaskConfig()
        {
            ToTable("tb_tasks");

            Property(p => p.Name).HasMaxLength(100).IsRequired();
            Property(p => p.Explain).HasMaxLength(256).IsRequired();
            Property(p => p.Content).HasMaxLength(2048);
            Property(p => p.Publisher).HasMaxLength(100);
        }
    }
}
