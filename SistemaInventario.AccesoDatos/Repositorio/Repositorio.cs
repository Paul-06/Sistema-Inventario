using Microsoft.EntityFrameworkCore;
using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using SistemaInventario.Modelos.Especificaciones;
using System.Linq.Expressions;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    // La implementación también es genérica
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        // Preparando atributos para inyección
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

       // Constructor
        public Repositorio(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>(); // Establecemos el tipo que manejará DbSet<> (Entidad)
        }

        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad);
        }
        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>>? filtro, string? incluirPropiedades, bool isTracking)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); // Select * from table where
            }

            if (incluirPropiedades != null)
            {
                foreach (var item in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item); // Obtener propiedades que vengan de otras tablas relacionadas
                }
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy, string? incluirPropiedades, bool isTracking)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); // Select * from table where
            }

            if (incluirPropiedades != null)
            {
                foreach (var item in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item); // Obtener propiedades que vengan de otras tablas relacionadas
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public PagedList<T> ObtenerTodosPaginado(Parametro parametro, Expression<Func<T, bool>>? filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? incluirPropiedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro); // Select * from table where
            }

            if (incluirPropiedades != null)
            {
                foreach (var item in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item); // Obtener propiedades que vengan de otras tablas relacionadas
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }

            return PagedList<T>.ToPagedList(query, parametro.PageNumber, parametro.PageSize);
        }

        public void Remover(T entidad)
        {
            dbSet.Remove(entidad);
        }

        public void RemoverRango(IEnumerable<T> entidades)
        {
            dbSet.RemoveRange(entidades);
        }
    }
}
