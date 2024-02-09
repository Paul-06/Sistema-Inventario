﻿using SistemaInventario.AccesoDatos.Data;
using SistemaInventario.AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaInventario.AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        // Atributo
        private readonly ApplicationDbContext _context;

        // Propiedades
        public IBodegaRepositorio Bodega { get; private set; }
        // Agregamos las nuevas intefaces
        public ICategoriaRepositorio Categoria { get; private set; }

        // Constructor
        public UnidadTrabajo(ApplicationDbContext context)
        {
            _context = context;
            // Inicializamos los repositorios con sus respectivas implementaciones
            Bodega = new BodegaRepositorio(_context);
            Categoria = new CategoriaRepositorio(_context);
        }

        public void Dispose()
        {
            _context.Dispose(); // Liberar memoria
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync();
        }
    }
}
