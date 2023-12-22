using MRA.Configurations.Initializer.Azure.AppConfig;
using MRA.Configurations.Initializer.Azure.Insight;
using MRA.Configurations.Initializer.Azure.KeyVault;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsProduction())
{
    builder.Configuration.ConfigureAzureKeyVault("MraInfrastructure");
    string appConfigConnectionString = builder.Configuration["AppConfigConnectionString"];
    builder.Configuration.AddAzureAppConfig(appConfigConnectionString);
    builder.Logging.AddApiApplicationInsights(builder.Configuration);
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// step 1:
builder.Configuration.SetBasePath(Path.Combine(builder.Environment.ContentRootPath, "Configurations"));

// step 2:
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var corsAllowedHosts = builder.Configuration.GetSection("MraInfrastructure-CORS").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS_POLICY", policyConfig =>
    {
        policyConfig.WithOrigins(corsAllowedHosts)
                    .AllowAnyHeader()
                    .AllowAnyMethod();
    });
});

builder.Services.AddOcelot();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CORS_POLICY");

// step 3:
app.UseOcelot().Wait();
app.Run();
