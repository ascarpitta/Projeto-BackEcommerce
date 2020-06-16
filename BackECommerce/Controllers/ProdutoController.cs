using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackECommerce.Controllers
{
    [Route("api/Produtos")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public ActionResult<List<Produto>> GetProduto()
        {
            return _produtoRepository.BuscarProdutos();
        }

        [HttpGet("Id/{produtoId}")]
        public ActionResult<Produto> GetProdutoById(string produtoId)
        {
            var produto = _produtoRepository.BuscarProduto(produtoId);

            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpGet("Usuario/{userId}")]
        public ActionResult<List<Produto>> GetProdutosByUser(string userId)
        {
            var produto = _produtoRepository.BuscarProdutosPorUsuario(userId);

            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpGet("Usuario/{userId}/{id}")]
        public ActionResult<Produto> GetProdutoByUser(string userId, string id)
        {
            var produto = _produtoRepository.BuscarProdutoPorUsuario(userId, id);

            if (produto == null)
            {
                return NotFound();
            }
            return produto;
        }

        [HttpGet("Busca/{busca}")]
        public ActionResult<List<Produto>> GetProdutoByName(string busca)
        {
            var produto = _produtoRepository.BuscarProdutosPorNome(busca);

            if (produto != null)
            {
                return produto;
            }
            return NotFound();
        }

        [HttpGet("MaiorPreco/{busca}")]
        public ActionResult<List<Produto>> GetProdutoByOrder1(string busca)
        {
            var produto = _produtoRepository.BuscarProdutoPorNomeOrdemMaiorPreco(busca);

            if (produto != null)
            {
                return produto;
            }
            return NotFound();
        }

        [HttpGet("MenorPreco/{busca}")]
        public ActionResult<List<Produto>> GetProdutoByOrder2(string busca)
        {
            var produto = _produtoRepository.BuscarProdutoPorNomeOrdemMenorPreco(busca);

            if (produto != null)
            {
                return produto;
            }
            return NotFound();
        }

        [HttpGet("Abc/{busca}")]
        public ActionResult<List<Produto>> GetProdutoByOrder3(string busca)
        {
            var produto = _produtoRepository.BuscarProdutoPorNomeOrdemAbc(busca);

            if (produto != null)
            {
                return produto;
            }
            return NotFound();
        }

        [HttpGet("Zyx/{busca}")]
        public ActionResult<List<Produto>> GetProdutoByOrder4(string busca)
        {
            var produto = _produtoRepository.BuscarProdutoPorNomeOrdemZyx(busca);

            if (produto != null)
            {
                return produto;
            }
            return NotFound();
        }

        [HttpGet("CadastroProduto/{idUsuario}/{nome}/{descricao}/{preco}/{frete}/{quantidade}/{categoria}/{marca}")]
        public ActionResult<Produto> PostProduto(string idUsuario, string nome, string descricao, double preco, double frete, int quantidade, string categoria, string marca)
        {
            Produto produto = new Produto
            {
                User = idUsuario,
                Name = nome,
                Description = descricao,
                Price = preco,
                Frete = frete,
                Quantity = quantidade,
                Category = categoria,
                Marca = marca,
                Ativo = true
            };

            _produtoRepository.CadastroProduto(produto);   
            return Ok();
        }

        [HttpGet("AlterarProduto/{userId}/{id}")]
        public IActionResult Put(string userId, string id, Produto produtoNovo)
        {
            var produto = _produtoRepository.AtualizarProduto(userId, id, produtoNovo);

            if (produto == null)
            {
                return NotFound();
            }            

            return Ok();
        }

        [HttpGet("AtivarProduto/{userId}/{id}")]
        public IActionResult AtivarProduto(string userId, string id)
        {
            var produto = _produtoRepository.AtivaProduto(userId, id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("InativarProduto/{userId}/{id}")]
        public IActionResult InativarProduto(string userId, string id)
        {
            var produto = _produtoRepository.InativaProduto(userId, id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}