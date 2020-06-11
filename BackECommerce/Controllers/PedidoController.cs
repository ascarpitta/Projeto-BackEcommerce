﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackECommerce.Controllers
{
    [Route("api/Pedido")]
    [ApiController]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository;
        public PedidoController(IPedidoRepository pedidoRepository)
        {
            _pedidoRepository = pedidoRepository;
        }

        [HttpGet]
        public ActionResult<List<Pedido>> GetPedido()
        {
            return _pedidoRepository.BuscarPedidos();
        }

        [HttpGet("BuscarPedido/{userId}/{pedidoId}")]
        public ActionResult<Pedido> GetPedidoPorUsuario(string userid, string pedidoId)
        {
            return _pedidoRepository.BuscarPedidoPorUsuario(userid, pedidoId);
        }

        [HttpGet("BuscarPedidos/{userId}")]
        public ActionResult<List<Pedido>> GetPedidosPorUsuario(string userid)
        {
            return _pedidoRepository.BuscarPedidosPorUsuario(userid);
        }

        [HttpGet("PagarPedido/{userId}/{pedidoId}")]
        public ActionResult<Pedido> GetPagarPedido(string userId, string pedidoId)
        {
            return _pedidoRepository.PagarPedido(userId, pedidoId);
        }

        [HttpGet("CancelarItemPedido/{userId}/{pedidoId}/{produtoId}")]
        public ActionResult<Pedido> GetCancelarItemPedido(string userId, string pedidoId, string produtoId)
        {
            return _pedidoRepository.AtualizarStatusPedidoCompra(userId, pedidoId, produtoId, 0);
        }

        [HttpGet("ReceberItemPedido/{userId}/{pedidoId}/{produtoId}")]
        public ActionResult<Pedido> GetReceberItemPedido(string userId, string pedidoId, string produtoId)
        {
            return _pedidoRepository.AtualizarStatusPedidoCompra(userId, pedidoId, produtoId, 1);
        }

        [HttpGet("CancelarItemPedidoVenda/{userId}/{pedidoId}/{produtoId}")]
        public ActionResult<Venda> CancelarItemPedidoVenda(string userId, string pedidoId)
        {
            return _pedidoRepository.AtualizarStatusPedido(userId, pedidoId, 0);
        }

        [HttpGet("ItemEmTransportePedidoVenda/{userId}/{pedidoId}/{produtoId}")]
        public ActionResult<Venda> ItemEmTransportePedidoVenda(string userId, string pedidoId)
        {
            return _pedidoRepository.AtualizarStatusPedido(userId, pedidoId, 1);
        }

        //[HttpGet("ExibirPedidosVenda/{userId}")]
        //public ActionResult<List<Venda>> GetExibirPedidosVenda(string userId)
        //{
        //    return _pedidoRepository.BuscarVendasPorUsuario(userId);
        //}

        [HttpGet("ExibirPedidoVenda/{userId}/{pedidoId}")]
        public ActionResult<Venda> GetExibirPedidoVenda(string userId, string pedidoId)
        {
            return _pedidoRepository.BuscarVendaPorUsuario(userId, pedidoId);
        }

        [HttpGet("GetExibirAndamentoVenda/{userId}")]
        public ActionResult<List<Venda>> GetExibirAndamentoVenda(string userId)
        {
            return _pedidoRepository.PedidosVendaEmAndamento(userId);
        }

        [HttpGet("GetExibirAndamentoCompra/{userId}")]
        public ActionResult<List<Pedido>> GetExibirAndamentoCompra(string userId)
        {
            return _pedidoRepository.PedidosCompraEmAndamento(userId);
        }
    }
}