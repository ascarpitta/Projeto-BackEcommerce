using BackECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        void CadastroUsuario(Usuario usuario);
        Usuario VerificarLogin(string email, string senha);
        string CriptografarSenha(string senha);
        List<Usuario> BuscarUsuarios();
        Usuario BuscarUsuario(string id);
        Usuario BuscarUsuarioPorEmail(string email);
        Usuario VerificarCpf(long cpf);
        Usuario VerificarEmail(string email);
        void AtualizarUsuario(string id, Usuario novoUsuario);
        Usuario AlterarSenha(string id, string senhaAntiga, string senhaNova);
        void DeletarUsuarioPorId(string id);
        void DeletarUsuarioPorEmail(string email);
        void AdicionarListaDesejo(string idUsuario, string idProduto);
        void RemoverListaDesejo(string idUsuario, string idProduto);
        Usuario AtivarUsuario(string userId);
        Usuario InativarUsuario(string userId);
        void RecuperarSenha(string email, long cpf);
    }
}
