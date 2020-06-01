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

        [HttpGet("BuscarPedido/{userId}")]
        public ActionResult<List<Pedido>> GetPedidosPorUsuario(string userid)
        {
            return _pedidoRepository.BuscarPedidosPorUsuario(userid);
        }

    }
}