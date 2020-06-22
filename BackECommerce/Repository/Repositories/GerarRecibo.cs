using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//ESSAS SAO AS BIBLIOTECAS QUE DEVEREMOS ADICIONAR EM NOSSO PROJETO
using System.IO;// A BIBLIOTECA DE ENTRADA E SAIDA DE ARQUIVOS

using iTextSharp;//E A BIBLIOTECA ITEXTSHARP E SUAS EXTENÇÕES
using iTextSharp.text;//ESTENSAO 1 (TEXT)
using iTextSharp.text.pdf;//ESTENSAO 2 (PDF)
using BackECommerce.Models;
using NUnit.Framework.Constraints;
using BackECommerce.Service;

namespace BackECommerce.Repository.Repositories
{
    public class GerarRecibo
    {    
        private readonly Font _fonteTitulo = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 22);
        private readonly Font _fonte = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 12);
        private readonly UsuarioService _usuarioService = new UsuarioService();       

        public void gerarReciboCompraProduto(Pedido pedCompra, Venda venda)
        {
            var usuario_compra = _usuarioService.GetUsuarioById(pedCompra.UserId);
            var usuario_venda = _usuarioService.GetUsuarioById(venda.UserIdVenda);
            Document doc = new Document(PageSize.A4); 
            doc.SetMargins(40, 40, 40, 80); 
            doc.AddCreationDate();

            string caminho = AppDomain.CurrentDomain.BaseDirectory.ToString() + "ReciboCompra.pdf";
            PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

            doc.Open();

            //Título
            Paragraph quebraLinha = new Paragraph("\n", _fonte);
            Paragraph titulo = new Paragraph("Recibo de Compra\n\n", _fonteTitulo);
            titulo.Alignment = Element.ALIGN_CENTER;
            doc.Add(titulo);

            //Dados Pedido
            PdfPTable tab_pedido = new PdfPTable(3);
            //Cabeçalho
            var cel_dt_pedido = new PdfPCell();
            var cel_dt_recibo = new PdfPCell();
            var cel_num_pedido = new PdfPCell();

            cel_dt_pedido.AddElement(new Paragraph("DATA PEDIDO", _fonte));
            cel_dt_recibo.AddElement(new Paragraph("DATA RECIBO", _fonte));
            cel_num_pedido.AddElement(new Paragraph("NÚMERO PEDIDO", _fonte));

            tab_pedido.AddCell(cel_dt_pedido);
            tab_pedido.AddCell(cel_dt_recibo);
            tab_pedido.AddCell(cel_num_pedido);

            //Dados
            double num = pedCompra.NumPedido;
            tab_pedido.AddCell(new PdfPCell(new Phrase(pedCompra.DataPedidoRealizado.ToString())));
            tab_pedido.AddCell(new PdfPCell(new Phrase(pedCompra.DataPagamentoConfirmado.ToString())));
            tab_pedido.AddCell(new PdfPCell(new Phrase(num.ToString())));

            //Cabeçalho
            var cel_vl_produto = new PdfPCell();
            var cel_vl_frete = new PdfPCell();
            var cel_vl_total = new PdfPCell();

            cel_vl_produto.AddElement(new Paragraph("VALOR COMPRA", _fonte));
            cel_vl_frete.AddElement(new Paragraph("VALOR FRETE", _fonte));
            cel_vl_total.AddElement(new Paragraph("VALOR FINAL", _fonte));

            tab_pedido.AddCell(cel_vl_produto);
            tab_pedido.AddCell(cel_vl_frete);
            tab_pedido.AddCell(cel_vl_total);

            //Dados
            tab_pedido.AddCell(new PdfPCell(new Phrase(venda.VlFinalCompra.ToString("0.00"))));
            tab_pedido.AddCell(new PdfPCell(new Phrase(venda.VlFreteCompra.ToString("0.00"))));
            tab_pedido.AddCell(new PdfPCell(new Phrase(venda.VlTotalCompra.ToString("0.00"))));

            doc.Add(tab_pedido);
            doc.Add(quebraLinha);
            //---------------------------------------------------------------------------------
            //Dados Comprador
            PdfPTable tab_comprador = new PdfPTable(2);

            //Cabeçalho
            var cel_nome_comprador = new PdfPCell();
            var cel_cpf_comprador = new PdfPCell();

            cel_nome_comprador.AddElement(new Paragraph("NOME COMPRADOR", _fonte));
            cel_cpf_comprador.AddElement(new Paragraph("CPF COMPRADOR", _fonte));

            tab_comprador.AddCell(cel_nome_comprador);
            tab_comprador.AddCell(cel_cpf_comprador);

            //Dados
            tab_comprador.AddCell(new PdfPCell(new Phrase(usuario_compra.Name)));
            tab_comprador.AddCell(new PdfPCell(new Phrase(usuario_compra.Cpf.ToString())));

            doc.Add(tab_comprador);
            doc.Add(quebraLinha);
            //---------------------------------------------------------------------------------
            //Dados Vendedor
            PdfPTable tab_vendedor = new PdfPTable(2);

            //Cabeçalho
            var cel_nome_vendedor = new PdfPCell();
            var cel_cpf_vendedor = new PdfPCell();

            cel_nome_vendedor.AddElement(new Paragraph("NOME VENDEDOR", _fonte));
            cel_cpf_vendedor.AddElement(new Paragraph("CPF VENDEDOR", _fonte));

            tab_vendedor.AddCell(cel_nome_vendedor);
            tab_vendedor.AddCell(cel_cpf_vendedor);

            //Dados
            tab_vendedor.AddCell(new PdfPCell(new Phrase(usuario_venda.Name)));
            tab_vendedor.AddCell(new PdfPCell(new Phrase(usuario_venda.Cpf.ToString())));

            doc.Add(tab_vendedor);
            doc.Add(quebraLinha);
            //---------------------------------------------------------------------------------
            //Dados Endereço
            PdfPTable tab_endereco = new PdfPTable(4);
            //Cabeçalho
            var cel_cidade = new PdfPCell();
            var cel_estado = new PdfPCell();
            var cel_cep = new PdfPCell();
            var cel_bairro = new PdfPCell();

            cel_cidade.AddElement(new Paragraph("CIDADE", _fonte));
            cel_estado.AddElement(new Paragraph("ESTADO", _fonte));
            cel_cep.AddElement(new Paragraph("CEP", _fonte));
            cel_bairro.AddElement(new Paragraph("BAIRRO", _fonte));

            tab_endereco.AddCell(cel_cidade);
            tab_endereco.AddCell(cel_estado);
            tab_endereco.AddCell(cel_cep);
            tab_endereco.AddCell(cel_bairro);
            //Dados
            tab_endereco.AddCell(new PdfPCell(new Phrase(pedCompra.Cidade)));
            tab_endereco.AddCell(new PdfPCell(new Phrase(pedCompra.Uf)));
            tab_endereco.AddCell(new PdfPCell(new Phrase(pedCompra.Cep)));
            tab_endereco.AddCell(new PdfPCell(new Phrase(pedCompra.Bairro)));

            //-----
            //Cabeçalho
            var cel_rua = new PdfPCell();
            var cel_numero = new PdfPCell();
            var cel_complemento = new PdfPCell();

            cel_rua.AddElement(new Paragraph("RUA", _fonte));
            cel_numero.AddElement(new Paragraph("NÚMERO", _fonte));
            cel_complemento.AddElement(new Paragraph("COMPLEMENTO", _fonte));

            tab_endereco.AddCell(cel_rua);
            tab_endereco.AddCell(cel_numero);
            tab_endereco.AddCell(cel_complemento);
            tab_endereco.AddCell(new PdfPCell(new Phrase(" ")));

            //Dados
            tab_endereco.AddCell(new PdfPCell(new Phrase(pedCompra.Rua)));
            tab_endereco.AddCell(new PdfPCell(new Phrase(pedCompra.Numero.ToString())));
            tab_endereco.AddCell(new PdfPCell(new Phrase(pedCompra.Complemento)));
            tab_endereco.AddCell(new PdfPCell(new Phrase(" ")));

            doc.Add(tab_endereco);
            doc.Add(quebraLinha);
            //---------------------------------------------------------------------------------
            //Produtos
            PdfPTable tab_produtos = new PdfPTable(5);

            var cel_produto = new PdfPCell();
            var cel_qtd = new PdfPCell();
            var cel_preco = new PdfPCell();
            var cel_frete = new PdfPCell();
            var cel_vl_final = new PdfPCell();

            cel_produto.AddElement(new Paragraph("PRODUTO", _fonte));
            cel_qtd.AddElement(new Paragraph("QTD", _fonte));
            cel_preco.AddElement(new Paragraph("PREÇO", _fonte));
            cel_frete.AddElement(new Paragraph("FRETE", _fonte));
            cel_vl_final.AddElement(new Paragraph("VALOR", _fonte));

            tab_produtos.AddCell(cel_produto);
            tab_produtos.AddCell(cel_qtd);
            tab_produtos.AddCell(cel_preco);
            tab_produtos.AddCell(cel_frete);
            tab_produtos.AddCell(cel_vl_final);

            tab_produtos.AddCell(new PdfPCell(new Phrase(venda.NomeProduto)));

            double t = venda.Quandidade;
            tab_produtos.AddCell(new PdfPCell(new Phrase(t.ToString())));
            tab_produtos.AddCell(new PdfPCell(new Phrase(venda.VlFinalCompra.ToString("0.00"))));
            tab_produtos.AddCell(new PdfPCell(new Phrase(venda.VlFreteCompra.ToString("0.00"))));
            tab_produtos.AddCell(new PdfPCell(new Phrase((venda.VlTotalCompra).ToString("0.00"))));            

            for (int i=0; i < 8; i++)
            {
                tab_produtos.AddCell(new PdfPCell(new Phrase(" ")));
            }

            tab_produtos.AddCell(new PdfPCell(new Phrase("VALOR FINAL:")));
            tab_produtos.AddCell(new PdfPCell(new Phrase(venda.VlTotalCompra.ToString("0.00"))));

            doc.Add(tab_produtos);
            doc.Add(quebraLinha);
            //---------------------------------------------------------------------------------
            doc.Close();
        }
    }
}
