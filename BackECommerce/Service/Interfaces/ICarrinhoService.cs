using BackECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackECommerce.Service.Interfaces
{
    public interface ICarrinhoService
    {
        List<Carrinho> GetCarrinho();
        Carrinho GetCarrinhoByUser(string id);
        Carrinho CreateCarrinho(Carrinho carrinho);
        void UpdateCarrinho(Carrinho carrinhoNovo, string id);
        void EndCarrinhoByUser(string userId);
        void EndCarrinhos();

    }
}
