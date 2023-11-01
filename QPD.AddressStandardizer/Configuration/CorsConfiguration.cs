namespace QPD.AddressStandardizer.Configuration
{
    public static class CorsConfiguration
    {
        private const string CORS_POLICY_NAME = "_allowAny";
        public static IServiceCollection AddAppCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CORS_POLICY_NAME, policy =>
                {
                    policy.AllowAnyOrigin();
                });
            });
            return services;
        }

        public static WebApplication UseAppCors(this WebApplication app)
        {
            app.UseCors(CORS_POLICY_NAME);
            return app;
        }
    }
}
