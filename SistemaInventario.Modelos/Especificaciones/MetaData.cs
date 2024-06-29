namespace SistemaInventario.Modelos.Especificaciones
{
    public class MetaData
    {
        public int TotalPages { get; set; }
        public int PageSize { get; set; } // Cantidad de elementos por pagina
        public int TotalCount { get; set; } // Total de registros
    }
}
