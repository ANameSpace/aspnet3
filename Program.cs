using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PSite3.Mappings;
using PSite3.Repositories.Implementations;
using PSite3.Repositories.Interfaces;
using PSite3.Services.Implementations;
using PSite3.Services.Interfaces;

namespace PSite3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "PSite3 API",
                    Version = "v1",
                    Description = "REST API интернет-магазина PSite3: товары, категории, корзины, " +
                                  "заказы, пользователи, роли и резервирования."
                });


                options.DocInclusionPredicate((_, api) =>
                    api.RelativePath?.StartsWith("api/", StringComparison.OrdinalIgnoreCase) == true);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddDbContext<PSite3.Models.ShopContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Connection"));
            });

            // Репозитории и сервисы регистрируются как Scoped: на каждый HTTP-запрос
            // создаётся один экземпляр, использующий Scoped DbContext. Это исключает
            // совместное использование DbContext между запросами и сохраняет единый
            // ChangeTracker до единственного SaveChangesAsync в рамках операции.
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();
            builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOrderItemService, OrderItemService>();
            builder.Services.AddScoped<IReservationService, ReservationService>();

            builder.Services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(builder.Environment.ContentRootPath, "DataProtection-Keys")))
                    .SetApplicationName("PSite3");


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    options.SlidingExpiration = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                });
            builder.Services.AddAuthorization();


            builder.Services.AddMemoryCache();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Swagger UI доступен только в режиме разработки: https://localhost:7063/swagger
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PSite3 API v1");
                    options.RoutePrefix = "swagger";
                    options.DocumentTitle = "PSite3 API";
                });
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
