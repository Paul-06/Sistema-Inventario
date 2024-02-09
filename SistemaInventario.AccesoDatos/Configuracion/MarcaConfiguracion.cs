using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaInventario.Modelos;
using System.Configuration;

namespace SistemaInventario.AccesoDatos.Configuracion
{
    // Esta clase se utilizará para configurar Marca (modelo que será una entidad en la BD)
    public class MarcaConfiguracion : IEntityTypeConfiguration<Marca> // Será buscada por Assembly (Fluent API)
    {
        // Este método se llama automáticamente por Entity Framework Core
        // para aplicar la configuración a la entidad Marca.
        public void Configure(EntityTypeBuilder<Marca> builder)
        {
            // Establecemos las propiedades de la entidad
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired(); // Boolean
        }
    }
}
