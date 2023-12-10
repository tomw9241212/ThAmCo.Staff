using Auth0.AspNetCore.Authentication;
using Polly;
using Polly.Extensions.Http;
using ThAmCo.Staff.Services;

namespace ThAmCo.Staff {
    public class Program {

        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            if (builder.Configuration.GetValue<bool>("WebServices:Orders:UseFake", false)) {
                builder.Services.AddTransient<IOrdersService, FakeOrdersService>();
            } else {
                builder.Services.AddHttpClient("TokenClient", client => {
                    client.BaseAddress = new Uri(builder.Configuration["Auth:Authority"]);
                });


                builder.Services.AddHttpClient("OrdersClient", client => {
                    client.BaseAddress = new Uri(builder.Configuration["WebServices:Orders:BaseAddress"]);
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

                builder.Services.AddTransient<IOrdersService, OrdersService>();
            }

            builder.Services.AddAuth0WebAppAuthentication(options => {
                options.Domain = builder.Configuration["Auth:Domain"];
                options.ClientId = builder.Configuration["Auth:ClientId"];
            });

            // Add services to the container.
            // TODO: Change from razor to controllers
            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment()) {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy() {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }


}