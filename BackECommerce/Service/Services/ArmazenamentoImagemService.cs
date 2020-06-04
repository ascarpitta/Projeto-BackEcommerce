using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using BackECommerce.Service.Interfaces;
using System.Globalization;
using System.Configuration;

namespace BackECommerce.Service.Services
{
    public class ArmazenamentoImagemService : IArmazenamentoImagemService
    {      
        public ArmazenamentoImagemService() 
        {
        }

        /// <summary>
        /// Faz o carregamento da imagem para o servidor em nuvem e retorna a URL dela
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string Carregar(string caminhoImagem)
        {
            Account account = new Account("hisd40dpu", "122483761947377", "PQfOwDI0EFUzf0GJzqtKlFWaGVs");
            Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(caminhoImagem),
                Folder = "images"
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            var url = uploadResult.SecureUri.ToString();
            return url;
        }
    }
}
