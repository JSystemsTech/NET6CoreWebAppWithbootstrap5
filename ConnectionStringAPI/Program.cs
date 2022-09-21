using ConnectionStringAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Text;

#if ApiConventions
[assembly: ApiConventionType(typeof(DefaultApiConventions))]
#endif

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.GetSection(ApplicationSettings.ConfigurationSection).Bind(Configuration.ApplicationSettings);
builder.Configuration.GetSection(JwtSettings.ConfigurationSection).Bind(Configuration.JwtSettings);
builder.Configuration.GetSection(SwaggerSettings.ConfigurationSection).Bind(Configuration.SwaggerSettings);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(Configuration.SwaggerSettings.Version, new OpenApiInfo
    {
        Title = Configuration.SwaggerSettings.APIName,
        Version = Configuration.SwaggerSettings.Version
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
builder.Services.AddScoped<UserManager>();
builder.Services.AddScoped<ConnectionStringManager>();
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
            ValidIssuer = Configuration.JwtSettings.Issuer,
            ValidAudience = Configuration.JwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.JwtSettings.EncryptionKey))
        };
    });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (Configuration.SwaggerSettings.UseDeveloperExceptionPage)
{
    app.UseDeveloperExceptionPage();
}
app.UseSwagger(c => c.RouteTemplate = $"{Configuration.SwaggerSettings.BasePath}/{{documentName}}/{Configuration.SwaggerSettings.APIName}.json");
if (Configuration.SwaggerSettings.UseSwaggerUI)
{
    //app.UseSwaggerUI(c => c.SwaggerEndpoint(Configuration.SwaggerSettings.SwaggerDocsPath, Configuration.SwaggerSettings.APIName));
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint($"/{Configuration.SwaggerSettings.BasePath}/{Configuration.SwaggerSettings.Version}/{Configuration.SwaggerSettings.APIName}.json", $"{Configuration.SwaggerSettings.APIName} - {Configuration.SwaggerSettings.Version}");
        c.RoutePrefix = Configuration.SwaggerSettings.BasePath;
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
