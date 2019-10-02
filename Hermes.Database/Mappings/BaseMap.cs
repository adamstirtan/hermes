using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Hermes.ObjectModel;

namespace Hermes.Database.Mappings
{
    public abstract class BaseMap<T> : IEntityMappingConfiguration<T> where T : BaseEntity
    {
        protected const string ColumnTypeId = "bigint";
        protected const string ColumnTypeInteger = "int";
        protected const string ColumnTypeBigInteger = "bigint";
        protected const string ColumnTypeBoolean = "bit";
        protected const string ColumnTypeDateTime = "datetime";
        protected const string ColumnTypeTime = "time";
        protected const string ColumnTypeMoney = "money";
        protected const string ColumnTypeDecimal = "decimal";
        protected const string ColumnTypeGeography = "geography";
        protected const string ColumnTypeBinary = "binary";
        protected const string ColumnTypeVariableBinary = "varbinary";

        protected const short ColumnLengthLarge = 1024;
        protected const short ColumnLengthStandard = 128;
        protected const short ColumnLengthSmall = 64;
        protected const short ColumnLengthTiny = 8;

        public abstract void Map(EntityTypeBuilder<T> builder);

        public void Map(ModelBuilder builder)
        {
            Map(builder.Entity<T>());
        }
    }
}