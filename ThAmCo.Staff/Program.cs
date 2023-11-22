using ThAmCo.Staff.Services;

namespace ThAmCo.Staff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Environment.IsDevelopment()) {
                builder.Services.AddTransient<IOrdersService, FakeOrdersService>();
            } else {
                builder.Services.AddHttpClient<IOrdersService, OrdersService>();
            }

            // Add services to the container.
            // TODO: Change from razor to controllers
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}