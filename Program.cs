using logistics_visualization_demo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace logistics_visualization_demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load Docker-specific configuration if running in container
            if (builder.Environment.IsProduction())
            {
                builder.Configuration.AddJsonFile("appsettings.Docker.json", optional: true, reloadOnChange: false);
            }

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<RecordContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("RecordContext")));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy
                    .WithOrigins("http://localhost:3000")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                var context = services.GetRequiredService<RecordContext>();

                // Log the connection string for debugging
                var connectionString = context.Database.GetDbConnection().ConnectionString;
                Console.WriteLine($"Using connection string: {connectionString}");

                // Run migrations if database exists; otherwise log an error
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Database migration failed: " + ex.Message);
                    throw;
                }

                DbInitializer.Initialize(context);
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("AllowFrontend");
            app.MapControllers();

            app.Run();
        }
    }
}
