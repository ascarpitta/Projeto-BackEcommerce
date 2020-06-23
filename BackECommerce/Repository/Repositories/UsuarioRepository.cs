using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using System;
using System.Collections.Generic;
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
                    usuario.Ativo = true;
                    _usuarioService.CreateUsuario(usuario);
                    _emailRepository.EnviarEmail(usuario.Email, "Cadastro realizado com sucesso!", $"Olá {usuario.Name}, seja bem vindo(a)");
                }                
            }
            //usuário já existe
        }

        public Usuario VerificarLogin(string email, string senha)
        {
            var user = _usuarioService.VerificarLogin(email, CriptografarSenha(senha));

            if (user != null)
            {
                if (user.Ativo)
                {
                    return user;
                }
            }
            return null;
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
                    _emailRepository.EnviarEmail(user.Email, "Usuário inativado", $"Caro {user.Name}, seu usuário foi reativado.");
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
                    _emailRepository.EnviarEmail(usuario.Email, "Usuário inativado", $"Caro {usuario.Name}, seu usuário foi inativado, para reativa-lo favor entrar no site https://proj-front-2020.herokuapp.com/login-usuario.html e selecionar a opção de reativar cadastro.");
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
            Random number = new Random();
            var usuario = BuscarUsuarioPorEmail(email);
            string newPassword = number.Next(100000, 999999).ToString();
            usuario.Password = CriptografarSenha(newPassword);
            AtualizarUsuario(usuario.Id, usuario);
            _emailRepository.EnviarEmail(usuario.Email, "Recuperação de senha", $"Olá {usuario.Name}, sua nova senha é {newPassword}");
        }
    }
}
