using System.Collections.Generic;
using BackECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using BackECommerce.Repository.Interfaces;

namespace BackECommerce.Controllers
{
    [Route("api/Usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuariosController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
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
            var user = _usuarioRepository.BuscarUsuarioPorEmail(email);
            if (user != null)
            {
                if (user.Cpf == cpf)
                {
                    _usuarioRepository.RecuperarSenha(email, cpf);
                    return Ok();
                }
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

        [HttpGet("CadastroUsuario/{email}/{cpf}/{nome}/{senha}")]
        public ActionResult<Usuario> PostUsuario(string email, long cpf, string nome, string senha)
        {
            Usuario usuario = new Usuario();
            usuario.Email = email;
            usuario.Cpf = cpf;
            usuario.Password = senha;
            usuario.Ativo = true;
            usuario.Name = nome;
            _usuarioRepository.CadastroUsuario(usuario);            
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

        [HttpGet("AlterarSenha/{id}/{senhaAntiga}/{senhaNova}")]
        public IActionResult AlterarSenha(string id, string senhaAntiga, string senhaNova)
        {
            var usuario = _usuarioRepository.AlterarSenha(id, senhaAntiga, senhaNova);

            if (usuario != null)
            {
                return Ok();
            }
            
            return NotFound();
        }

        [HttpGet("Ativar/{email}/{cpf}")]
        public IActionResult AtivarUsuario(string email, long cpf)
        {
            var usuario = _usuarioRepository.AtivarUsuario(email, cpf);

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

    }
}