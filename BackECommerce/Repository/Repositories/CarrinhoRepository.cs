using BackECommerce.Models;
using BackECommerce.Repository.Interfaces;
using BackECommerce.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Repository.Repositories
{
    public class CarrinhoRepository : ICarrinhoRepository
    {
        private readonly CarrinhoService _carrinhoService = new CarrinhoService();
        private readonly IProdutoRepository _produtoRepository = new ProdutoRepository();
        private readonly IUsuarioRepository _usuarioRepository = new UsuarioRepository();
        private readonly IPedidoRepository _pedidoRepository = new PedidoRepository();
        private readonly IEmailRepository _emailRepository = new EmailRepository();
        private readonly IEnderecoRepository _enderecoRepository = new EnderecoRepository();
        
        public Carrinho BuscarCarrinhoPorUsuario(string id)
        {
            return _carrinhoService.GetCarrinhoByUser(id);
        }

        public List<Carrinho> BuscarCarrinhos()
        {
            return _carrinhoService.GetCarrinho();
        }

        public Carrinho AddEndereco(string userId, string enderecoId)
        {
            if (userId.Length == 24 && enderecoId.Length == 24)
            {
                var carrinho = _carrinhoService.GetCarrinhoByUser(userId);
                var endereco = _enderecoRepository.BuscarEndereco(enderecoId);
                if (endereco != null)
                {
                    if (endereco.User == userId)
                    {
                        carrinho.EnderecoId = enderecoId;
                        _carrinhoService.UpdateCarrinho(carrinho, userId);
                        return _carrinhoService.GetCarrinhoByUser(userId);
                    }
                }
                           
            }
            return null;
        }

        public Carrinho AddProduto(string userId, string produtoId)
        {
            var usuario = _usuarioRepository.BuscarUsuario(userId);

            if (usuario != null)
            {                
                var produto = _produtoRepository.BuscarProduto(produtoId);
                if (produto != null)
                {
                    if (produto.User != userId)
                    {
                        if (produto.Quantity > 0 && produto.Ativo)
                        {
                            ProdutosCarrinho novoProduto = new ProdutosCarrinho();

                            novoProduto.IdProduto = produtoId;
                            novoProduto.NameProduto = produto.Name;
                            novoProduto.Quantidade = 1;
                            novoProduto.Preco = produto.Price;
                            novoProduto.Frete = produto.Frete;
                            novoProduto.IdUserVenda = produto.User;
                            novoProduto.url_imagem = produto.url_imagem;

                            var carrinho = _carrinhoService.GetCarrinhoByUser(usuario.Id);
                            if (carrinho == null)
                            {
                                Carrinho novoCarrinho = new Carrinho();
                                novoCarrinho.UserId = userId;
                                novoCarrinho.Produtos = new List<ProdutosCarrinho>();
                                novoCarrinho.Produtos.Add(novoProduto);

                                carrinho = _carrinhoService.CreateCarrinho(novoCarrinho);
                            }
                            else
                            {
                                bool existe = false;

                                foreach (ProdutosCarrinho prod in carrinho.Produtos)
                                {
                                    if (prod.IdProduto == produtoId)
                                    {
                                        existe = true;
                                        if (prod.Quantidade + 1 <= produto.Quantity)
                                        {
                                            prod.Quantidade += 1;
                                        }
                                        else
                                        {
                                            return null;
                                        }
                                    }
                                }

                                if (!existe)
                                {
                                    carrinho.Produtos.Add(novoProduto);
                                }

                                _carrinhoService.UpdateCarrinho(carrinho, userId);
                            }
                            if (produto.Carrinhos == null)
                            {
                                produto.Carrinhos = new List<string>();
                            }

                            produto.Carrinhos.Add(carrinho.Id);
                            _produtoRepository.AtualizarProduto(produto.User, produto.Id, produto);
                            return _carrinhoService.GetCarrinhoByUser(userId);
                        }
                    }
                }
            }
            return null;
        }

        public Carrinho AlterarQuantProduto(string userId, string produtoId, int tipo)
        {
            var usuario = _usuarioRepository.BuscarUsuario(userId);
            bool existe = false;
            if (usuario != null)
            {
                var carrinho = BuscarCarrinhoPorUsuario(usuario.Id);
                var produto = _produtoRepository.BuscarProduto(produtoId);

                if (carrinho != null && produto != null)
                {
                    if (produto.Quantity > 0 && produto.Ativo)
                    {
                        foreach (ProdutosCarrinho prod in carrinho.Produtos)
                        {
                            if (prod.IdProduto == produtoId)
                            {
                                if (tipo == 1) //aumenta qtde
                                {
                                    if (prod.Quantidade + 1 <= produto.Quantity)
                                    {
                                        existe = true;
                                        prod.Quantidade += 1;
                                    }
                                    else
                                    {
                                        return null;
                                    }
                                }
                                else //diminui qtde
                                {
                                    if (prod.Quantidade - 1 == 0)
                                    {
                                        produto.Carrinhos.Remove(carrinho.Id);
                                        _produtoRepository.AtualizarProduto(produto.User, produto.Id, produto);
                                        return RemoverProduto(userId, produtoId);
                                    }
                                    else
                                    {
                                        existe = true;
                                        prod.Quantidade -= 1;
                                    }
                                }
                            }
                            else
                            {
                                return RemoverProduto(userId, produtoId);
                            }
                        }
                        if (!existe)
                        {
                            return null;
                        }
                        _carrinhoService.UpdateCarrinho(carrinho, userId);
                        return carrinho;
                    }
                }
            }
            return null;
        }        

        public void RemoverCarrinhoPorUsuario(string userId)
        {
            _carrinhoService.EndCarrinhoByUser(userId);
        }

        public Carrinho RemoverProduto(string userId, string produtoId)
        {
            var usuario = _usuarioRepository.BuscarUsuario(userId);
            if (usuario != null)
            {
                var carrinho = BuscarCarrinhoPorUsuario(usuario.Id);
                var produto = _produtoRepository.BuscarProduto(produtoId);

                if (carrinho != null && produto != null)
                {
                    foreach(ProdutosCarrinho p in carrinho.Produtos)
                    {
                        if (p.IdProduto == produtoId)
                        {
                            carrinho.Produtos.Remove(p);

                            _carrinhoService.UpdateCarrinho(carrinho, userId);

                            produto.Carrinhos.Remove(carrinho.Id);
                            _produtoRepository.AtualizarProduto(produto.User, produto.Id, produto);

                            if (carrinho.Produtos.Count() == 0)
                            {
                                RemoverCarrinhoPorUsuario(userId);
                            }
                            return carrinho;
                        }
                    }                    
                }
            }
            return null;
        }

        public Pedido FinalizarCarrinho(string userId)
        {
            var usuario = _usuarioRepository.BuscarUsuario(userId);

            if (usuario != null)
            {
                var carrinho = BuscarCarrinhoPorUsuario(usuario.Id);

                if (carrinho != null)
                {
                    if (carrinho.EnderecoId != null)
                    {
                        var pedido = _pedidoRepository.CriarPedido(usuario.Id, carrinho);
                        
                        if (pedido != null)
                        {
                            RemoverCarrinhoPorUsuario(userId);
                            _emailRepository.EnviarEmail(usuario.Email, "Pedido realizado com sucesso!", $"Caro(a) {usuario.Name}, \n\nseu pedido está sendo processado em nosso sistema e ficará pronto em breve. \n\nObrigado!");
                            return pedido;
                        }                        
                    }
                }
            }
            return null;
        }

        public Carrinho BuscarCarrinho(string carrinhoId)
        {
            if (carrinhoId.Length == 24)
            {
                return _carrinhoService.GetCarrinhoById(carrinhoId);
            }
            return null;
        }
    }
}
