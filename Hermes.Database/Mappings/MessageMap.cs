using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Hermes.ObjectModel;

namespace Hermes.Database.Mappings
{
    internal sealed class MessageMap : BaseMap<Message>
    {
        public override void Map(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Messages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.User)
                .IsRequired()
                .HasMaxLength(ColumnLengthStandard)
                .HasColumnName("User");

            builder.Property(x => x.Content)
                .IsRequired()
                .HasColumnName("Content");

            builder.Property(x => x.Created)
                .IsRequired()
                .HasColumnType(ColumnTypeDateTime)
                .HasColumnName("Created");
        }
    }
}