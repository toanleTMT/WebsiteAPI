using Applications.Kafkas.Configs;
using Applications.Kafkas.CustomJsons;
using kafkaConsumer.Handlers;
using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.TypedHandler;
using Services.Kafka.QueueMessageService;

namespace kafkaConsumer
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

            services.AddControllers(mvcOptions => mvcOptions.EnableEndpointRouting = false);

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

            services.AddSingleton<IQueueMessageService, QueueMessageService>();

            // TODO: Kafka
            services.AddSingleton<KafkaConfig>();

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

            // Start Kafka
            StartKafka(app);
        }

        private void ConfigureKafka(IServiceCollection services)
        {
            services.AddKafka(kafka => kafka
                .UseConsoleLog()
                .AddKafkaConsumerCluster(Configuration));
        }

        private void StartKafka(IApplicationBuilder app)
        {
            var kafkaBus = app.ApplicationServices.CreateKafkaBus();

            var lifetime = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
            lifetime.ApplicationStarted.Register(() => kafkaBus.StartAsync(lifetime.ApplicationStopped));
        }
    }

    public static class KafkaConfiguration
    {
        public static IKafkaConfigurationBuilder AddKafkaConsumerCluster(this IKafkaConfigurationBuilder clusterConfig, IConfiguration configuration)
        {
            var kafkaSection = configuration.GetSection("KafkaConfigs:KafkaAppSetting");
            var kafkaConfigs = kafkaSection.Get<KafkaConfig>();

            clusterConfig = clusterConfig
                .AddCluster(cluster => cluster
                    .WithBrokers(new[] { kafkaConfigs.BootstrapServers })
                    .AddConsumer(consumer => consumer
                        .Topic(kafkaConfigs.TopicName)
                            .WithGroupId(kafkaConfigs.GroupId)
                            .WithName($"{kafkaConfigs.GroupId}:{Environment.MachineName}")
                            .WithBufferSize(200)
                            .WithWorkersCount(10)
                            .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                            .AddMiddlewares(middlewares => middlewares
                                .AddSerializer(
                                    resolver => new CustomJsonSerializer(),
                                    resolver => new CustomMessageTypeResolver())
                                .AddTypedHandlers(handlers => handlers
                                        .WithHandlerLifetime(InstanceLifetime.Singleton)
                                        .AddHandler<QueueMessageKafkaHandler>())

                            )
                        )
                );

            return clusterConfig;
        }

    }
}
