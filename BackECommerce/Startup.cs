using BackECommerce.Repository.Interfaces;
using BackECommerce.Repository.Repositories;
using BackECommerce.Repository;
using BackECommerce.Service;
using BackECommerce.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http.Features;
using System;

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
            services.AddSingleton<IUsuarioRepository, UsuarioRepository>();
            services.AddSingleton<IProdutoRepository, ProdutoRepository>();
            services.AddSingleton<IEnderecoRepository, EnderecoRepository>();
            services.AddSingleton<ICarrinhoRepository, CarrinhoRepository>();
            services.AddSingleton<IPedidoRepository, PedidoRepository>();
            services.AddSingleton<IEmailRepository, EmailRepository>();

            services.Configure<FormOptions>(options =>
            {
                options.MemoryBufferThreshold = Int32.MaxValue;
            });

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
