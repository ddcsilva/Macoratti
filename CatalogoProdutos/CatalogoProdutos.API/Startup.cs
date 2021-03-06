using CatalogoProdutos.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoProdutos.API
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
            string mySqlConnectionString = Configuration.GetConnectionString("CatalogoProdutoDB");

            services.AddDbContext<DataContext>(options => options.UseMySql(mySqlConnectionString, ServerVersion.AutoDetect(mySqlConnectionString)));

            // Para evitar a refer?ncia c?clica, ? necess?rio adicionar a biblioteca da Microsoft Newtonsoft.json e adaptar
            services.AddControllers().AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Adiciona o middleware para redirecionar para https
            app.UseHttpsRedirection();

            // Adiciona o middleware de roteamento
            app.UseRouting();

            // Adiciona o middleware que habilita a autoriaza??o
            app.UseAuthorization();

            // Adiciona o middleware que executa o endpoint do request atual
            app.UseEndpoints(endpoints =>
            {
                // Adiciona os endpoints para as Actions dos controladores sem especificar rotas
                endpoints.MapControllers();
            });
        }
    }
}
