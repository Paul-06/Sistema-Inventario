﻿using SistemaInventario.Modelos.Especificaciones;
using System.Linq.Expressions;

namespace SistemaInventario.AccesoDatos.Repositorio.IRepositorio
{
    // Lo hacemos genérico y establecemos que reciba solamente clases
    public interface IRepositorio<T> where T : class
    {
        Task<T> Obtener(int id);

        Task<IEnumerable<T>> ObtenerTodos(
            Expression<Func<T, bool>>? filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? incluirPropiedades = null,
            bool isTracking = true // Obtener datos y modificarlos también
        );

        PagedList<T> ObtenerTodosPaginado(
            Parametro parametro, Expression<Func<T, bool>>? filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string? incluirPropiedades = null,
            bool isTracking = true
        );

        Task<T> ObtenerPrimero(
            Expression<Func<T, bool>>? filtro = null,
            string? incluirPropiedades = null,
            bool isTracking = true // Obtener datos y modificarlos también
        );

        Task Agregar(T entidad);

        // Los métodos para eliminar no deben ser asíncronos
        void Remover(T entidad);

        void RemoverRango(IEnumerable<T> entidades);
    }
}
