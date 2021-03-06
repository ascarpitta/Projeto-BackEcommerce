﻿using System.Collections.Generic;
using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using BackECommerce.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BackECommerce.Controllers
{
    [Route("api/Carrinho")]
    [ApiController]
    public class CarrinhoController : ControllerBase
    {
        private readonly ICarrinhoRepository _carrinhoRepository;
        private readonly IProdutoRepository _produtoRepository = new ProdutoRepository();
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
        public int PutAlterarQtd(string userId, string produtoId)
        {
            _carrinhoRepository.AlterarQuantProduto(userId, produtoId, 1);

            var produto = _produtoRepository.BuscarProduto(produtoId);

            return produto.Quantity;
        }

        [HttpGet("DiminuirProduto/{userId}/{produtoId}")]
        public ActionResult<Carrinho> PutDiminuirQtd(string userId, string produtoId)
        {
            var carrinho = _carrinhoRepository.AlterarQuantProduto(userId, produtoId, -1);

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
            return pedido;
        }

        [HttpDelete("DeletarCarrinho/{userId}")]
        public void Delete(string userId)
        {
            _carrinhoRepository.RemoverCarrinhoPorUsuario(userId);
        }
    }
}
