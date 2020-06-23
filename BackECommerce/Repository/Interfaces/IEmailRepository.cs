namespace BackECommerce.Repository.Interfaces
{
    public interface IEmailRepository
    {
        void EnviarEmail(string para, string assunto, string conteudo);
    }
}
