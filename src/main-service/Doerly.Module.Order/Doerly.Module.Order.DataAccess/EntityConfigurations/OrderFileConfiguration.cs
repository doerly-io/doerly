using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Doerly.Module.Order.DataAccess.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Doerly.Module.Order.DataAccess.Entities;

namespace Doerly.Module.Order.DataAccess.EntityConfigurations;
public class OrderFileConfiguration : IEntityTypeConfiguration<OrderFile>
{
    public void Configure(EntityTypeBuilder<OrderFile> builder)
    {
        builder.ToTable(DbConstants.Tables.OrderFile, DbConstants.OrderSchema);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Path).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
        builder.Property(x => x.Type).IsRequired().HasMaxLength(50);

        builder.HasOne(x => x.Order).WithMany(x => x.OrderFiles).HasForeignKey(x => x.OrderId);
    }
}