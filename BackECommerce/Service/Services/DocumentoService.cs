using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using BackECommerce.Service.Interfaces;
using System.Globalization;
using System.Configuration;
using BackECommerce.Configs;

namespace BackECommerce.Service.Services
{
    public class DocumentoService : IDocumentoService
    {
        private static readonly CloudinaryConfig CloudinaryConfig = new CloudinaryConfig();
        private static readonly Account Account = new Account(CloudinaryConfig.Cloud, CloudinaryConfig.ApiKey, CloudinaryConfig.ApiSecret);
        private static readonly Cloudinary Cloudinary = new Cloudinary(Account);

        public DocumentoService() 
        {
        }

        /// <summary>
        /// Faz o carregamento da imagem para o servidor em nuvem e retorna a URL dela
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string CarregarImagem(string caminhoArquivo)
        {
            //Account account = new Account("hisd40dpu", "122483761947377", "PQfOwDI0EFUzf0GJzqtKlFWaGVs");
            //Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(caminhoArquivo),
                Folder = "images"
            };
            var uploadResult = Cloudinary.Upload(uploadParams);
            var url = uploadResult.SecureUri.ToString();
            return url;
        }

        /// <summary>
        /// Faz o carregamento da NFe para o servidor em nuvem e retorna a URL dela
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string CarregarNFe(string caminhoArquivo)
        {
            //Account account = new Account("hisd40dpu", "122483761947377", "PQfOwDI0EFUzf0GJzqtKlFWaGVs");
            //Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(caminhoArquivo),
                Folder = "NFe's"
            };
            var uploadResult = Cloudinary.Upload(uploadParams);
            var url = uploadResult.SecureUri.ToString();
            return url;
        }
    }
}
