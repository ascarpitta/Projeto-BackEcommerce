using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Ocsp;

namespace BackECommerce.Controllers
{
    [Route("api/Enderecos")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {
        private readonly IEnderecoRepository _enderecoRepository;
        public EnderecoController(IEnderecoRepository enderecoRepository)
        {
            _enderecoRepository = enderecoRepository;
        }

        [HttpGet]
        public ActionResult<List<Endereco>> GetEndereco()
        {
            return _enderecoRepository.BuscarEnderecos();
        }

        [HttpGet("Id/{enderecoId}")]
        public ActionResult<Endereco> GetEnderecoById(string enderecoId)
        {
            var endereco = _enderecoRepository.BuscarEndereco(enderecoId);

            if (endereco == null)
            {
                return NotFound();
            }
            return endereco;
        }

        [HttpGet("{userId}")]
        public ActionResult<List<Endereco>> GetEnderecosByUserId(string userId)
        {
            var endereco = _enderecoRepository.BuscarEnderecoPorUsuario(userId);

            if (endereco == null)
            {
                return NotFound();
            }
            return endereco;
        }

        [HttpGet("ViaCep/{cep}")]
        public ActionResult<EnderecoViaCep> GetEnderecosByCep(string cep)
        {
            var endereco = _enderecoRepository.BuscarEnderecoPorCep(cep);

            if (endereco == null)
            {
                return NotFound();
            }
            return endereco;
        }

        [HttpGet("CadastroEndereco/{idUsuario}/{nome}/{cep}/{uf}/{cidade}/{bairro}/{rua}/{numero}/{complemento}")]
        public ActionResult<Endereco> PostEndereco(string idUsuario, string nome, string cep, string uf, string cidade, string bairro, string rua, int numero, string complemento)
        {
            Endereco endereco = new Endereco();
            endereco.User = idUsuario;
            endereco.NomeEndereco = nome;
            endereco.Cep = cep;
            endereco.Uf = uf;
            endereco.Cidade = cidade;
            endereco.Bairro = bairro;
            endereco.Rua = rua;
            endereco.Numero = numero;
            endereco.Complemento = complemento; 

            _enderecoRepository.CadastroEndereco(endereco); 
            return Ok();
        }

        [HttpGet("CadastroEndereco/{idUsuario}/{nome}/{cep}/{uf}/{cidade}/{bairro}/{rua}/{numero}")]
        public ActionResult<Endereco> PostEnderecoSemComplemento(string idUsuario, string nome, string cep, string uf, string cidade, string bairro, string rua, int numero)
        {
            Endereco endereco = new Endereco();
            endereco.User = idUsuario;
            endereco.NomeEndereco = nome;
            endereco.Cep = cep;
            endereco.Uf = uf;
            endereco.Cidade = cidade;
            endereco.Bairro = bairro;
            endereco.Rua = rua;
            endereco.Numero = numero;

            _enderecoRepository.CadastroEndereco(endereco);
            return Ok();
        }

        [HttpPost("AlterarEndereco")]
        public IActionResult PutSemComplemento()
        {
            Endereco enderecoNovo = new Endereco();
            enderecoNovo.User = Request.Form["idUser"].First();
            enderecoNovo.NomeEndereco = Request.Form["nomeEndereco"].First();
            enderecoNovo.Cep = Request.Form["cep"].First();
            enderecoNovo.Uf = Request.Form["uf"].First();
            enderecoNovo.Cidade = Request.Form["cidade"].First();
            enderecoNovo.Bairro = Request.Form["bairro"].First();
            enderecoNovo.Rua = Request.Form["rua"].First();
            enderecoNovo.Numero = Convert.ToInt32(Request.Form["numero"].First());

            var endereco = _enderecoRepository.AtualizarEndereco(enderecoNovo.User, Request.Form["idEndereco"].First(), enderecoNovo);

            if (endereco == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet("ExcluirEndereco/{userId}/{id}")]
        public IActionResult GetExcluirEndereco(string userId, string id)
        {
            var endereco = _enderecoRepository.BuscarEndereco(id);

            if (endereco == null)
            {
                return NotFound();
            }

            _enderecoRepository.RemoverEndereco(id);

            return Ok();
        }
    }
}
