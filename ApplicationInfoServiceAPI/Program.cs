using ApplicationInfoServiceAPI;
using ApplicationInfoServiceAPI.DomainLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Data.Common;
using System.Reflection;
using System.Text;

#if ApiConventions
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
#endif

var builder = WebApplication.CreateBuilder(args);
DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
builder.Configuration.GetSection(ApplicationSettings.ConfigurationSection).Bind(ApplicationConfiguration.ApplicationSettings);
builder.Configuration.GetSection(JwtSettings.ConfigurationSection).Bind(ApplicationConfiguration.JwtSettings);
builder.Configuration.GetSection(SwaggerSettings.ConfigurationSection).Bind(ApplicationConfiguration.SwaggerSettings);



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(ApplicationConfiguration.SwaggerSettings.Version, new OpenApiInfo
    {
        Title = ApplicationConfiguration.SwaggerSettings.APIName,
        Version = ApplicationConfiguration.SwaggerSettings.Version
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });


});
builder.Services.AddScoped<IConnectionStringManager, ConnectionStringManager>();
builder.Services.AddScoped<AuthenticationTokenServiceDbConnection>();
builder.Services.AddScoped<UserManager>();
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = ApplicationConfiguration.JwtSettings.Issuer,
            ValidAudience = ApplicationConfiguration.JwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ApplicationConfiguration.JwtSettings.EncryptionKey))
        };
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (ApplicationConfiguration.SwaggerSettings.UseDeveloperExceptionPage)
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger(c => c.RouteTemplate = $"{ApplicationConfiguration.SwaggerSettings.BasePath}/{{documentName}}/{ApplicationConfiguration.SwaggerSettings.APIName}.json");
if (ApplicationConfiguration.SwaggerSettings.UseSwaggerUI)
{
    //app.UseSwaggerUI(c => c.SwaggerEndpoint(Configuration.SwaggerSettings.SwaggerDocsPath, Configuration.SwaggerSettings.APIName));
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/{ApplicationConfiguration.SwaggerSettings.BasePath}/{ApplicationConfiguration.SwaggerSettings.Version}/{ApplicationConfiguration.SwaggerSettings.APIName}.json", $"{ApplicationConfiguration.SwaggerSettings.APIName} - {ApplicationConfiguration.SwaggerSettings.Version}");
        c.RoutePrefix = ApplicationConfiguration.SwaggerSettings.BasePath;
    });
}

//if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
//{
//    app.UseDeveloperExceptionPage();
//    app.UseSwagger();
//    app.UseSwaggerUI();
//    //app.UseSwaggerUI(c =>
//    //{
//    //    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
//    //    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Authentication Token Service");
//    //});
//}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();
app.Run();
