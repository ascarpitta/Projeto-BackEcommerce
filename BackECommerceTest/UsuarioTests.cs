using Microsoft.VisualStudio.TestTools.UnitTesting;
using BackECommerce.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using BackECommerce.Models;

namespace BackECommerceTest
{
    [TestClass]
    public class UsuarioTests
    {

        private readonly UsuarioRepository _usuarioRepository;
        private readonly Usuario _usuarioTeste;
        public UsuarioTests()
        {
            _usuarioRepository = new UsuarioRepository();
            _usuarioTeste = _usuarioRepository.BuscarUsuarioPorEmail("teste@hotmail.com");
        }

//Cadastro de usuário
        [Test] //com email e cpf não cadastrados
        public void CadastroUsuarioTest()
        {

                Usuario usuarioTeste = new Usuario();
                usuarioTeste.Name = "teste cadastro";
                usuarioTeste.Cpf = 88216545320;
                usuarioTeste.Email = "teste_cadastro@hotmail.com";
                usuarioTeste.Password = "123454";

                _usuarioRepository.CadastroUsuario(usuarioTeste);
                Usuario usuarioresult = _usuarioRepository.BuscarUsuarioPorEmail("teste_cadastro@hotmail.com");

                Assert.AreEqual("teste cadastro", usuarioresult.Name);

                _usuarioRepository.DeletarUsuarioPorId(usuarioresult.Id);

        }

//Métodos auxiliares
        [Test] //verificar email cadastrado
        public void BuscaUsuarioEmailSucesso()
        {

                Usuario usuarioresult = _usuarioRepository.VerificarEmail("teste@hotmail.com");

                Assert.AreEqual("teste@hotmail.com", usuarioresult.Email);
        }

        [Test] //verificar email não cadastrado
        public void BuscaUsuarioEmailErro()
        {

                Usuario usuarioresult = _usuarioRepository.BuscarUsuarioPorEmail("teste_erro@hotmail.com");

                Assert.AreEqual(null, usuarioresult);
        }

        [Test] //verificar cpf cadastrado
        public void BuscaUsuarioCpfSucesso()
        {

                Usuario usuarioresult = _usuarioRepository.VerificarCpf(36462363614);

                Assert.AreEqual(36462363614, usuarioresult.Cpf);
            
        }

        [Test] //verificar cpf não cadastrado
        public void BuscaUsuarioCpfErro()
        {

            Usuario usuarioresult = _usuarioRepository.VerificarCpf(28409897865);

            if(usuarioresult != null)
            {
                Assert.Fail();
            }

        }

//Alteração de usuário
        [Test] 
        public void AlterarUsuarioSucesso()
        {

            Usuario usuarioTeste = new Usuario();
            usuarioTeste.Name = "teste cadastro";
            usuarioTeste.Cpf = 88216545320;
            usuarioTeste.Email = "teste_cadastro@hotmail.com";
            usuarioTeste.Password = "123454";

            _usuarioRepository.CadastroUsuario(usuarioTeste);

                
            Usuario usuarioresult = _usuarioRepository.BuscarUsuarioPorEmail("teste_cadastro@hotmail.com");

                
            Usuario usuarioAlterado = _usuarioRepository.AlterarSenha(usuarioresult.Id, "123454", "23456");

            Usuario usuarioAlteradoRsult = _usuarioRepository.BuscarUsuario(usuarioresult.Id);

            Assert.AreEqual(usuarioAlterado.Password, usuarioAlteradoRsult.Password);

            _usuarioRepository.DeletarUsuarioPorId(usuarioresult.Id);
        }

//Verificar login
        [Test] //com email e senha corretos
        public void VerificaLoginSucesso()
        {
            Usuario usuarioTeste = new Usuario();
            usuarioTeste.Name = "teste cadastro";
            usuarioTeste.Cpf = 88216545320;
            usuarioTeste.Email = "teste_cadastro@hotmail.com";
            usuarioTeste.Password = "123454";
            usuarioTeste.Ativo = true;

            _usuarioRepository.CadastroUsuario(usuarioTeste);


            Usuario usuarioresult = _usuarioRepository.VerificarLogin("teste_cadastro@hotmail.com", "123454");

            if (usuarioresult == null)
            {
                Assert.Fail();
            }
            else
            {
                _usuarioRepository.DeletarUsuarioPorId(usuarioresult.Id);
            }            
        }

        [Test] //com senha incorreta
        public void VerficaLoginSenhaErro()
        {
            Usuario usuarioTeste = new Usuario();
            usuarioTeste.Name = "teste cadastro";
            usuarioTeste.Cpf = 88216545320;
            usuarioTeste.Email = "teste_cadastro@hotmail.com";
            usuarioTeste.Password = "123454";

            _usuarioRepository.CadastroUsuario(usuarioTeste);

            Usuario usuarioEmailresult = _usuarioRepository.BuscarUsuarioPorEmail("teste_cadastro@hotmail.com");
            Usuario usuarioresult = _usuarioRepository.VerificarLogin("teste_cadastro@hotmail.com", "23454");

            if (usuarioresult != null)
            {
                Assert.Fail();
            }
            else
            {
                _usuarioRepository.DeletarUsuarioPorId(usuarioEmailresult.Id);
            }            
        }
    }
}