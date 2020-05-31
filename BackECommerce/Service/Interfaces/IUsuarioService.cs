using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Service.Interfaces
{
    public interface IUsuarioService
    {
        List<Usuario> GetUsuario();        
        Usuario GetUsuarioById(string id);
        Usuario GetUsuarioByCpf(long cpf);
        Usuario GetUsuarioByEmail(string email);
        Usuario CreateUsuario(Usuario usuario);
        void UpdateUsuario(string id, Usuario usuarioNovo);
        bool VerificarEmail(string email);
        bool VerificarCpf(long cpf);
        Usuario VerificarLogin(string email, string senha);

        void DeleteUsuarioById(string id);
        void DeleteUsuarioByEmail(string email);
    }
}
