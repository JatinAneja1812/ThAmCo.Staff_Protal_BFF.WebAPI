using Microsoft.AspNetCore.Authentication.JwtBearer;
using Service.Classes.Company;
using Service.Classes.Products;
using Service.Classes.UserReviews;
using Service.Classes.Users;
using Service.Interfaces.Company;
using Service.Interfaces.Customers;
using Service.Interfaces.Products;
using Service.Interfaces.UserReviews;

namespace ThAmCo.Staff_Protal_BFF.WebAPI
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                    }
                );
            });

            // Configure JWT authentication.
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = _configuration["Jwt:Authority"];
                options.Audience = _configuration["Jwt:Audience"];
            });

            // Configure the database context
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            // Add controllers
            services.AddControllers();

            // Add services
            services.AddScoped<ICustomerReviews, CustomerReviewsFake>();
            services.AddScoped<IProductsService, ProductsServiceFake>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddHttpClient<UserService>();
            services.AddTransient<IUserService, UserService>();

            // Add API endpoint exploration and Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Add the file provider
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseRouting();

            //JWT Bearer authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

