using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    //Injeção de dependência de dois repositórios apenas para didática, apenas o repositório específico é suficiente
    private readonly IUnitOfWork _uof;
    private readonly ILogger<ProdutosController> _logger;

    public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet("produtos/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
        var produtos = _uof.ProdutoRepository.GetProdutoPorCategoria(id);
        if (produtos is null)
        {
            _logger.LogWarning($"Produtos não encontrados");
            return NotFound("Produtos não encontrados");
        }
        return Ok(produtos);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Produto>> Get()
    {
        var produtos = _uof.ProdutoRepository.GetAll();
        if (produtos is null)
        {
            _logger.LogWarning($"Produtos não encontrados...");
            return NotFound("Produtos não encontrados...");
        }
        return Ok(produtos);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    public ActionResult<Produto> Get(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);
        if (produto is null)
        {
            _logger.LogWarning($"Produto não encontrado...");
            return NotFound("Produto não encontrado...");
        }
        return Ok(produto);
    }

    [HttpPost]
    public ActionResult Post(Produto produto)
    {
        if (produto is null)
        {
            _logger.LogWarning($"Produto inválido...");
            return BadRequest("Produto inválido...");
        }
        var novoProduto = _uof.ProdutoRepository.Create(produto);
        _uof.Commit();

        return new CreatedAtRouteResult("ObterProduto",
            new { id = novoProduto.ProdutoId }, produto);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Produto produto)
    {
        if (id != produto.ProdutoId)
        {
            _logger.LogWarning($"Produto inválido...");
            return BadRequest("Produto inválido...");
        }

        var produtoAtualizado = _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(produtoAtualizado);
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if(produto is null)
        {
            _logger.LogWarning($"Produto inválido...");
            return NotFound("Produto inválido...");
        }

        var produtoDeletado = _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();
        return Ok(produtoDeletado);
    }
}
