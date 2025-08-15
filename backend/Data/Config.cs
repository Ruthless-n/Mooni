using Microsoft.EntityFrameworkCore;
using DotNetEnv;

namespace backend.Data
{
    public static class Config
    {
        public static void LoadDb(IServiceCollection services)
        {
            Env.Load();

            var connString = Environment.GetEnvironmentVariable("CONNECTION_STRING");

            if (string.IsNullOrEmpty(connString))
            {
                throw new Exception("ERROR: No .env found.");
            }

            services.AddDbContext<_dbContext>(options =>
                options.UseNpgsql(connString));
        }

        public static string LoadJwt()
        {
            var secretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");

            if (string.IsNullOrEmpty(secretKey))
                throw new Exception("ERROR: No .env found.");

            return secretKey;
        }

        public static string LoadJwtExpiration()
        {
            var expiration = Environment.GetEnvironmentVariable("JWT_EXPIRATION");

            if (string.IsNullOrEmpty(expiration))
                throw new Exception("ERROR: No .env found.");

            return expiration;
        }
    }
}
