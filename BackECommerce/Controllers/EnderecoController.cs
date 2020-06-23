﻿using System.Collections.Generic;
using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("AlterarEndereco/{id}/{nome}/{cep}/{uf}/{cidade}/{bairro}/{rua}/{numero}/{complemento}")]
        public IActionResult Put(string id, string nome, string cep, string uf, string cidade, string bairro, string rua, int numero, string complemento)
        {
            var end = _enderecoRepository.BuscarEndereco(id);

            if (end != null)
            {                
                end.NomeEndereco = nome;
                end.Cep = cep;
                end.Uf = uf;
                end.Cidade = cidade;
                end.Bairro = bairro;
                end.Rua = rua;
                end.Numero = numero;
                end.Complemento = complemento;

                var endereco = _enderecoRepository.AtualizarEndereco(end.User, end.Id, end);

                if (endereco != null)
                {
                    return Ok();
                }                
            }
            return NotFound();
        }

        [HttpGet("AlterarEndereco/{id}/{nome}/{cep}/{uf}/{cidade}/{bairro}/{rua}/{numero}")]
        public IActionResult PutSemComplemento(string id, string nome, string cep, string uf, string cidade, string bairro, string rua, int numero)
        {
            var end = _enderecoRepository.BuscarEndereco(id);

            if (end != null)
            {
                end.NomeEndereco = nome;
                end.Cep = cep;
                end.Uf = uf;
                end.Cidade = cidade;
                end.Bairro = bairro;
                end.Rua = rua;
                end.Numero = numero;

                var endereco = _enderecoRepository.AtualizarEndereco(end.User, end.Id, end);

                if (endereco != null)
                {
                    return Ok();
                }
            }
            return NotFound();
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
