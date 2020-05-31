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

        // POST: api/Pedido
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Pedido/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
//Pedido CriarPedido(string userId, Carrinho carrinho);
//void AtualizarPedido(Pedido pedidoNovo, string id);
//void DeletarPedidoPorUsuario(string userId);