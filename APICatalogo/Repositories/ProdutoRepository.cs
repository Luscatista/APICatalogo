using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APICatalogo.Repositories;

public class ProdutoRepository : Repository<Produto>, IProdutoRepository
{
    public ProdutoRepository(AppDbContext context) : base(context)
    {
    }
    //public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
    //{
    //    return GetAll()
    //        .OrderBy(p => p.Nome)
    //        .Skip((produtosParameters.pageNumber - 1) * produtosParameters.PageSize)
    //        .Take(produtosParameters.PageSize).ToList();
    //}

    public PagedList<Produto> GetProdutos(ProdutosParameters produtosParams)
    {
        var produtos = GetAll().OrderBy(p => p.ProdutoId).AsQueryable();
        var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParams.pageNumber, produtosParams.PageSize);

        return produtosOrdenados;
    }
    public IEnumerable<Produto> GetProdutoPorCategoria(int id)
    {
        return GetAll().Where(c => c.CategoriaId == id);
    }
}
