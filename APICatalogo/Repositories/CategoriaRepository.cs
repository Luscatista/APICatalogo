using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> GetCategorias()
        {
            return _context.Categorias.ToList();
        }
        public Categoria GetCategoria(int id)
        {
            return _context.Categorias.FirstOrDefault(c => c.CategoriaId == id);
        }
        public Categoria Create(Categoria categoria)
        {
            if(categoria is null)
            {
                throw new ArgumentException(nameof(categoria));
            }

            _context.Categorias.Add(categoria);
            _context.SaveChanges();

            return categoria;
        }
        public Categoria Update(Categoria categoria)
        {
            if (categoria is null)
            {
                throw new ArgumentException(nameof(categoria));
            }

            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();

            return categoria;
        }
        public Categoria Delete(int id)
        {
            var categotia = _context.Categorias.Find(id);

            _context.Categorias.Remove(categotia);
            _context.SaveChanges();

            return categotia;
        }
    }
}
