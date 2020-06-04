using System.Collections.Generic;
using BackECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using BackECommerce.Service.Interfaces;
using BackECommerce.Repository.Interfaces;
using BackECommerce.Repository.Repositories;
using System.Security.Cryptography;
using System.Text;
using System;

namespace BackECommerce.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private EmailRepository _emailRepository;
        public UsuariosController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _emailRepository = new EmailRepository();
        }

        [HttpGet]
        public ActionResult<List<Usuario>> GetUser()
        {
            return _usuarioRepository.BuscarUsuarios();
        }

        [HttpGet("{email}/{senha}")]
        public ActionResult<Usuario> GetUserByLogin(string email, string senha)
        {
            var usuario = _usuarioRepository.VerificarLogin(email, senha);

            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        [HttpGet("Recuperacao/{email}/{cpf}")]
        public ActionResult<Usuario> GetRecuperacao(string email, long cpf)
        {
            var cpfOk = _usuarioRepository.VerificarCpf(cpf);
            var emailOk = _usuarioRepository.VerificarEmail(email);

            var user = _usuarioRepository.BuscarUsuarioPorEmail(email);

            if (cpfOk != null && emailOk != null && user != null)
            {
                //chamar função de recuperação de senha
                Usuario newUser = user;
                string newPassword = GenerateHash();
                string criptedPassword = _usuarioRepository.CriptografarSenha(newPassword);
                newUser.Password = criptedPassword;
                _usuarioRepository.AtualizarUsuario(user.Id, newUser);
                _emailRepository.EnviarEmail(user.Email, "Senha alterada com sucesso!", $"Olá {user.Name}, sua nova senha é {newPassword}");
                return Ok();
            }
            return NotFound();
            
        }

        [HttpGet("Id/{id}")]
        public ActionResult<Usuario> GetUserById(string id)
        {
            var usuario = _usuarioRepository.BuscarUsuario(id);

            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        [HttpGet("Cpf/{cpf}")]
        public ActionResult<Usuario> GetUserByCpf(long cpf)
        {
            var usuario = _usuarioRepository.VerificarCpf(cpf);

            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        [HttpGet("Email/{email}")]
        public ActionResult<Usuario> GetUserByEmail(string email)
        {
            var usuario = _usuarioRepository.VerificarEmail(email);

            if (usuario == null)
            {
                return NotFound();
            }
            return usuario;
        }

        [HttpGet("ListaDesejo/{id}")]
        public ActionResult<List<ListaDesejo>> ListaDesejo(string id)
        {
            var usuario = _usuarioRepository.BuscarUsuario(id);
            if (usuario != null)
            {
                return usuario.ListaDesejos;
            }
            return NotFound();
        }

        [HttpPost("CadastroUsuario/{email}/{cpf}/{nome}/{senha}")]
        public ActionResult<Usuario> PostUsuario(string email, long cpf, string nome, string senha)
        {
            Usuario usuario = new Usuario();
            usuario.Email = email;
            usuario.Cpf = cpf;
            usuario.Password = senha;
            usuario.Ativo = true;
            usuario.Name = nome; //Adicao leo, provavelmente a amandinha esqueceu de colocar pra preencher o nome
            _usuarioRepository.CadastroUsuario(usuario);
            _emailRepository.EnviarEmail(usuario.Email, "Cadastro realizado com sucesso!", $"Olá {usuario.Name}, seja bem vindo(a)");
            return Ok();
        }

        [HttpGet("AlterarUsuario/{id}")]
        public IActionResult Put(string id, Usuario usuarioNovo)
        {
            var usuario = _usuarioRepository.BuscarUsuario(id);
            if (usuario != null)
            {
                _usuarioRepository.AtualizarUsuario(id, usuarioNovo);

                return Ok();
            }
            return NotFound();
        }

        [HttpPost("AlterarSenha/{id}/{senhaAntiga}/{senhaNova}")]
        public IActionResult AlterarSenha(string id, string senhaAntiga, string senhaNova)
        {
            var usuario = _usuarioRepository.AlterarSenha(id, senhaAntiga, senhaNova);

            if (usuario != null)
            {
                return Ok();
            }
            
            return NotFound();
        }

        [HttpGet("Ativar/{id}")]
        public IActionResult AtivarUsuario(string id)
        {
            var usuario = _usuarioRepository.AtivarUsuario(id);

            if (usuario != null)
            {
                return Ok();
            }
            
            return NotFound();
        }

        [HttpGet("Desativar/{id}")]
        public IActionResult DesativarUsuario(string id)
        {
            var usuario = _usuarioRepository.InativarUsuario(id);

            if (usuario != null)
            {
                return Ok();
            }
            
            return NotFound();            
        }

        [HttpGet("AddListaDesejo/{id}/{idProduto}")]
        public IActionResult AddListaDesejo(string id, string idProduto)
        {
            var usuario = _usuarioRepository.BuscarUsuario(id);

            if (usuario != null)
            {
                _usuarioRepository.AdicionarListaDesejo(id, idProduto);
                return Ok();
            }
            return NotFound();
        }

        [HttpGet("RemoverListaDesejo/{id}/{idProduto}")]
        public IActionResult RemoverListaDesejo(string id, string idProduto)
        {
            var usuario = _usuarioRepository.BuscarUsuario(id);

            if (usuario != null)
            {
                _usuarioRepository.RemoverListaDesejo(id, idProduto);
                return Ok();
            }
            return NotFound();
        }

        [HttpDelete("Id/{id}")]
        public void DeleteUserById(string id)
        {
            _usuarioRepository.DeletarUsuarioPorId(id);
        }

        [HttpDelete("Email/{email}")]
        public void DeleteUserByEmail(string email)
        {
            _usuarioRepository.DeletarUsuarioPorEmail(email);
        }

        public string GenerateHash()
        { 
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(new Random().Next(1000, 10000).ToString()));  
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < 10; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}