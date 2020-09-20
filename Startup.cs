using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PACOMUrlShortener.Controllers;
using PACOMUrlShortener.Models;

namespace PACOMUrlShortener
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
            //
            services.AddControllersWithViews();

            //
            services.AddDbContext<SqlDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("PacomUrlShortener")));
            services.AddScoped<UrlshortenersController, UrlshortenersController>();

            //
            services.AddRazorPages();

            //
            services.AddCors();

            // Register the Swagger generator - from test2.
            services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            //
            app.UseCors(options => { options.AllowAnyOrigin(); });

            //
            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //
                endpoints.MapDefaultControllerRoute();

                //
                endpoints.MapRazorPages();

            });
        }
    }
}
