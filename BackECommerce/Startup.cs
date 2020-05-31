using BackECommerce.Repository.Interfaces;
using BackECommerce.Repository.Repositories;
using BackECommerce.Repository;
using BackECommerce.Service;
using BackECommerce.Service.Interfaces;
using BackECommerce.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BackECommerce
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddSingleton<IUsuarioService, UsuarioService>();
            services.AddSingleton<IUsuarioRepository, UsuarioRepository>();

            services.AddSingleton<IProdutoService, ProdutoService>();
            services.AddSingleton<IProdutoRepository, ProdutoRepository>();

            services.AddSingleton<IEnderecoService, EnderecoService>();
            services.AddSingleton<IEnderecoRepository, EnderecoRepository>();

            services.AddSingleton<ICarrinhoService, CarrinhoService>();
            services.AddSingleton<ICarrinhoRepository, CarrinhoRepository>();

            services.AddSingleton<IPedidoService, PedidoService>();
            services.AddSingleton<IPedidoRepository, PedidoRepository>();

            services.AddCors();

            services.AddControllers();
            services.AddScoped<UsuarioService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }           

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(option => option.AllowAnyOrigin());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
