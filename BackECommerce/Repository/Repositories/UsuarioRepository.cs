using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackECommerce.Service;
using System.Security.Cryptography;
using System.Text;

namespace BackECommerce.Repository.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly ProdutoRepository _produtoRepository = new ProdutoRepository();
        private readonly EmailRepository _emailRepository = new EmailRepository();
        
        public void CadastroUsuario(Usuario usuario)
        {            
            if (_usuarioService.VerificarEmail(usuario.Email))
            {
                if (_usuarioService.VerificarCpf(usuario.Cpf))
                {
                    usuario.Password = CriptografarSenha(usuario.Password);
                    _usuarioService.CreateUsuario(usuario);
                    _emailRepository.EnviarEmail(usuario.Email, "Cadastro realizado com sucesso!", $"Olá {usuario.Name}, seja bem vindo(a)");
                }                
            }
            //usuário já existe
        }

        public Usuario VerificarLogin(string email, string senha)
        {
            return _usuarioService.VerificarLogin(email, CriptografarSenha(senha));
        }

        public string CriptografarSenha(string senha)
        {
            string chaveCripto;
            Byte[] cript = System.Text.ASCIIEncoding.ASCII.GetBytes(senha);
            chaveCripto = Convert.ToBase64String(cript);
            return chaveCripto;
        }
        
        public List<Usuario> BuscarUsuarios()
        {
            return _usuarioService.GetUsuario();
        }

        public Usuario BuscarUsuario(string id)
        {
            if (id.Length == 24)
            {
                return _usuarioService.GetUsuarioById(id);
            }
            return null;
        }

        public Usuario VerificarCpf(long cpf)
        {
            return _usuarioService.GetUsuarioByCpf(cpf);
        }

        public Usuario VerificarEmail(string email)
        {
            return _usuarioService.GetUsuarioByEmail(email);
        }

        public void AtualizarUsuario(string id, Usuario novoUsuario)
        {
            if (id.Length == 24)
            {
                _usuarioService.UpdateUsuario(id, novoUsuario);
            }            
        }

        public Usuario AlterarSenha(string id, string senhaAntiga, string senhaNova)
        {
            if (id.Length == 24)
            {
                var usuario = BuscarUsuario(id);

                if (usuario != null)
                {
                    if (usuario.Password == CriptografarSenha(senhaAntiga))
                    {
                        usuario.Password = CriptografarSenha(senhaNova);
                        AtualizarUsuario(id, usuario);
                        _emailRepository.EnviarEmail(usuario.Email, "Senha alterada com sucesso!", $"Olá {usuario.Name}, sua senha acaba de ser alterada! Caso essa alteração não tenha sido feita por você, inicie o processo de recuperação de senha em nosso site.");
                        return usuario;
                    }
                }
            }
            return null;
        }

        public void DeletarUsuarioPorId(string id)
        {
            if (id.Length == 24)
            {
                _usuarioService.DeleteUsuarioById(id);
            }
        }

        public void DeletarUsuarioPorEmail(string email)
        {
            _usuarioService.DeleteUsuarioByEmail(email);
        }

        public void AdicionarListaDesejo(string idUsuario, string idProduto)
        {
            bool existe = false;
            if (idProduto.Length == 24 && idUsuario.Length == 24)
            {
                var usuario = BuscarUsuario(idUsuario);
                var produto = _produtoRepository.BuscarProduto(idProduto);
                if (produto != null)
                {
                    if (usuario.ListaDesejos == null)
                    {
                        usuario.ListaDesejos = new List<ListaDesejo>();
                    }
                    
                    foreach (ListaDesejo item in usuario.ListaDesejos)
                    {
                        if (item.IdProduto == idProduto)
                        {
                            existe = true;
                        }
                    }

                    if (!existe)
                    {
                        ListaDesejo novo = new ListaDesejo(idProduto, produto.Name);

                        usuario.ListaDesejos.Add(novo);

                        AtualizarUsuario(idUsuario, usuario);
                    }
                    //item já add na lista
                }
                //produto não existe
            }
        }

        public void RemoverListaDesejo(string idUsuario, string idProduto)
        {
            if (idProduto.Length == 24 && idUsuario.Length == 24)
            {
                var usuario = BuscarUsuario(idUsuario);

                if (usuario.ListaDesejos != null)
                {
                    
                    usuario.ListaDesejos.RemoveAll(x => x.IdProduto == idProduto);
                    

                    AtualizarUsuario(idUsuario, usuario);
                }
            }
        }

        public Usuario AtivarUsuario(string email, long cpf)
        {
            var user = BuscarUsuarioPorEmail(email);

            if (user != null)
            {
                if (user.Cpf == cpf)
                {
                    user.Ativo = true;
                    AtualizarUsuario(user.Id, user);
                    return user;
                }
            }

            return null;
        }

        public Usuario InativarUsuario(string userId)
        {
            if (userId.Length == 24)
            {
                var usuario = BuscarUsuario(userId);

                if (usuario != null)
                {
                    usuario.Ativo = false;
                    AtualizarUsuario(userId, usuario);
                    return usuario;
                }
            }
            return null;
        }

        public Usuario BuscarUsuarioPorEmail(string email)
        {
            return _usuarioService.GetUsuarioByEmail(email);
        }

        public void RecuperarSenha(string email, long cpf)
        {
            var usuario = BuscarUsuarioPorEmail(email);
            string newPassword = GenerateHash();
            usuario.Password = CriptografarSenha(newPassword);
            AtualizarUsuario(usuario.Id, usuario);
            _emailRepository.EnviarEmail(usuario.Email, "Recuperação de senha", $"Olá {usuario.Name}, sua nova senha é {newPassword}");
        }

        public static string GenerateHash()
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
