using BackECommerce.Models;
using BackECommerce.Service.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BackECommerce.Service
{
	public class UsuarioService : IUsuarioService
    {		
		private readonly IMongoCollection<Usuario> _usuarios;

		public UsuarioService()
		{
			MongoClient client = new MongoClient("mongodb+srv://UserAdmin:projetocalebe@clusterecommerce-onzbe.mongodb.net/test?retryWrites=true&w=majority");
//#if DEBUG
//			IMongoDatabase database = client.GetDatabase("ProjetoEcommerceTest");
//#else
			IMongoDatabase database = client.GetDatabase("ProjetoEcommerce");
//#endif
			_usuarios = database.GetCollection<Usuario>("Users");
		}

		public List<Usuario> GetUsuario()
		{
			return _usuarios.Find(usuario => true).ToList();
		}	

		public Usuario GetUsuarioById(string id)
		{
			return _usuarios.Find<Usuario>(usuario => usuario.Id == id).FirstOrDefault();
		}

		public Usuario CreateUsuario(Usuario usuario)
		{
			usuario.CreatedAt = DateTime.Now;
			_usuarios.InsertOne(usuario);
			return usuario;
		}

		public void UpdateUsuario(string id, Usuario usuarioNovo)
		{
			_usuarios.ReplaceOne(usuario => usuario.Id == id, usuarioNovo);
		}

		public bool VerificarEmail(string email)
		{
			var x = _usuarios.Find<Usuario>(user => user.Email == email).FirstOrDefault();
			if (x == null)
			{
				return true;
			}
			return false;
		}

		public bool VerificarCpf(long cpf)
		{
			var x = _usuarios.Find<Usuario>(user => user.Cpf == cpf).FirstOrDefault();
			if (x == null)
			{
				return true;
			}
			return false;
		}

		public Usuario VerificarLogin(string email, string senha)
		{
			return _usuarios.Find<Usuario>(user => user.Email == email && user.Password == senha).FirstOrDefault();
		}

		public Usuario GetUsuarioByCpf(long cpf)
		{
			return _usuarios.Find<Usuario>(usuario => usuario.Cpf == cpf).FirstOrDefault();
		}

		public Usuario GetUsuarioByEmail(string email)
		{
			return _usuarios.Find<Usuario>(usuario => usuario.Email == email).FirstOrDefault();
		}

		public void DeleteUsuarioById(string id)
		{
			_usuarios.DeleteOne<Usuario>(usuario => usuario.Id == id);
		}

		public void DeleteUsuarioByEmail(string email)
		{
			_usuarios.DeleteOne<Usuario>(usuario => usuario.Email == email);
		}
	}
}
