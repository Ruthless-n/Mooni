using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace backend.Data
{
    public static class Config
    {
        public static void LoadDb(IServiceCollection services)
        {
            Env.Load();

            var connString = Environment.GetEnvironmentVariable("CONN_STRING");

            if (string.IsNullOrEmpty(connString))
            {
                throw new Exception("ERROR: No .env found.");
            }

            services.AddDbContext<_dbContext>(options =>
                options.UseNpgsql(connString)); // UseNpgsql agora está disponível devido à diretiva de namespace adicionada  
        }
    }
}
