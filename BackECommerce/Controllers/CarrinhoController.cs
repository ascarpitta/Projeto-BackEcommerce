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
    [Route("api/Carrinho")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinhoRepository _carrinhoRepository;
        public CarrinhoController(ICarrinhoRepository carrinhoRepository)
        {
            _carrinhoRepository = carrinhoRepository;
        }

        [HttpGet]
        public ActionResult<List<Carrinho>> GetCarrinho()
        {
            return _carrinhoRepository.BuscarCarrinhos();
        }

        [HttpGet("UserId/{userId}")]
        public ActionResult<Carrinho> GetCarrinhoByUserId(string userId)
        {
            var carrinho = _carrinhoRepository.BuscarCarrinhoPorUsuario(userId);

            if (carrinho == null)
            {
                return NotFound();
            }
            return carrinho;
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
            //Carrinho CriarCarrinho(string produtoId);
        }

        [HttpGet("AddProduto/{userId}/{produtoId}")]
        public ActionResult<Carrinho> PutAddProduto(string userId, string produtoid)
        {
            var carrinho = _carrinhoRepository.AddProduto(userId, produtoid);

            if (carrinho == null)
            {
                return NotFound();
            }

            return carrinho;
        }

        [HttpGet("AddEndereco/{userId}/{enderecoId}")]
        public ActionResult<Carrinho> PutAddEndereco(string userId, string enderecoId)
        {
            var carrinho = _carrinhoRepository.AddEndereco(userId, enderecoId);

            if (carrinho == null)
            {
                return NotFound();
            }

            return carrinho;
        }

        [HttpGet("RemoverProduto/{userId}/{produtoId}")]
        public ActionResult<Carrinho> PutRemoverProduto(string userId, string produtoid)
        {
            var carrinho = _carrinhoRepository.RemoverProduto(userId, produtoid);

            if (carrinho == null)
            {
                return NotFound();
            }

            return carrinho;
        }

        [HttpGet("AumentarProduto/{userId}/{produtoId}")]
        public ActionResult<Carrinho> PutAlterarQtd(string userId, string produtoId)
        {
            var carrinho = _carrinhoRepository.AumentarQuantProduto(userId, produtoId);

            if (carrinho == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("DiminuirProduto/{userId}/{produtoId}")]
        public ActionResult<Carrinho> PutDiminuirQtd(string userId, string produtoId)
        {
            var carrinho = _carrinhoRepository.DiminuirQuantProduto(userId, produtoId);

            if (carrinho == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpGet("FinalizarCarrinho/{userId}")]
        public ActionResult<Pedido> PutFinalizarCarrinho(string userId)
        {
            var pedido = _carrinhoRepository.FinalizarCarrinho(userId);

            if (pedido == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("DeletarCarrinho/{userId}")]
        public void Delete(string userId)
        {
            _carrinhoRepository.RemoverCarrinhoPorUsuario(userId);
        }
    }
}
