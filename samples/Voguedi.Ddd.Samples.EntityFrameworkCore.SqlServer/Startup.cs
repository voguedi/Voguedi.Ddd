using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Application.DataObjects;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Model;
using Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer.Domain.Repositories;

namespace Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer
{
    public class Startup
    {
        #region Public Properties

        public IConfiguration Configuration { get; }

        #endregion

        #region Ctors

        public Startup(IConfiguration configuration) => Configuration = configuration;

        #endregion

        #region Public Methods

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddVoguedi(s =>
            {
                s.UseAutoMapper(o =>
                {
                    o.CreateMap<NoteDataObject, Note>();
                    o.CreateMap<Note, NoteDataObject>();
                });
                s.UseSqlServerEntityFrameworkCore<NoteDbContext>(@"Server=(localdb)\MSSQLLocalDB;Database=Samples;Trusted_Connection=True");
                s.UseEventBus();
            });
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info { Title = "Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(s => s.SwaggerEndpoint("/swagger/v1/swagger.json", "Voguedi.Ddd.Samples.EntityFrameworkCore.SqlServer"));
        }

        #endregion
    }
}
