using Applications.Kafkas.Configs;
using KafkaFlow;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using WebsiteKafka.Kafkas;
using WebsiteKafka.Kafkas.Messages;

namespace WebsiteKafka
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .WithOrigins("*")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // TODO: Kafka
            services.AddSingleton<KafkaConfig>();
            services.AddSingleton<IKafkaProducerHandle, SendMessageKafkaProducer>();


            var kafkaSection = Configuration.GetSection("KafkaConfigs:KafkaAppSetting");
            var kafkaProducerConfig = kafkaSection.Get<KafkaConfig>();
            services.AddSingleton(x => kafkaProducerConfig);
            ConfigureKafka(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseRequestLocalization();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = string.Empty;
            });

            // Start Kafka
            StartKafka(app);
        }

        private void ConfigureKafka(IServiceCollection services)
        {
            services.AddKafka(kafka => kafka
                .UseConsoleLog());
        }

        private void StartKafka(IApplicationBuilder app)
        {
            var kafkaBus = app.ApplicationServices.CreateKafkaBus();

            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() => kafkaBus.StartAsync(lifetime.ApplicationStopped));
        }
    }
}
