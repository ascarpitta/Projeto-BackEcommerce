using System.Collections.Generic;
using System.Linq;
using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using BackECommerce.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.IO;

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

        [HttpGet("{userId}")]
        public ActionResult<List<Produto>> GetProduto(string userId)
        {
            return _produtoRepository.BuscarProdutosLogado(userId);
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

        [HttpPost("Imagem/Armazenar/{id}")]
        public ActionResult<Produto> StoreProductImage(string id)
        {
            DocumentoService documentoService = new DocumentoService();
            var image = Request.Form.Files.First();
            string filePath = string.Empty;

            if (image.Length <= 0)
            {
                return NotFound();
            }

            using (var target = new MemoryStream())
            {
                image.CopyTo(target);
                byte[] imageBytes = target.ToArray();
                using (var fs = new FileStream(image.FileName, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(imageBytes, 0, imageBytes.Length);
                    filePath = fs.Name;
                }
            }
            string public_url = documentoService.CarregarImagem(filePath);

            if (filePath != string.Empty)
            {
                System.IO.File.Delete(filePath);
            }

            var product = _produtoRepository.BuscarProduto(id);

            product.url_imagem = public_url;

            return _produtoRepository.AtualizarProduto(product.User, product.Id, product);         
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

            var prod = _produtoRepository.CadastroProduto(produto);  
            if (prod == null)
            {
                return NotFound();
            }
            return prod;
        }

        [HttpGet("AlterarProduto/{userId}/{id}/{nome}/{descricao}/{preco}/{frete}/{quantidade}/{categoria}/{marca}")]
        public ActionResult<Produto> Put(string userId, string id, string nome, string descricao, double preco, double frete, int quantidade, string categoria, string marca)
        {
            var prod = _produtoRepository.BuscarProdutoPorUsuario(userId, id);

            if (prod != null)
            {
                prod.Name = nome;
                prod.Description = descricao;
                prod.Price = preco;
                prod.Frete = frete;
                prod.Quantity = quantidade;
                prod.Category = categoria;
                prod.Marca = marca;
                prod.Ativo = true;
                var produto = _produtoRepository.AtualizarProduto(userId, id, prod);

                if (produto != null)
                {
                    return produto;                    
                }                
            }
            return NotFound();
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

        [HttpGet("Filtro/{categoria}")]
        public ActionResult<List<Produto>> Filtro(string categoria)
        {
            var produto = _produtoRepository.Filtro(categoria);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }
    }
}