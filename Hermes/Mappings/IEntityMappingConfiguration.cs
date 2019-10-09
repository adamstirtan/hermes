using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Hermes.Models;

namespace Hermes.Database.Mappings
{
    public interface IEntityMappingConfiguration
    {
        void Map(ModelBuilder builder);
    }

    public interface IEntityMappingConfiguration<T> : IEntityMappingConfiguration where T : BaseEntity
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}