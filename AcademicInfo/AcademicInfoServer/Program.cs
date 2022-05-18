
using AcademicInfoServer.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.Text;

using Newtonsoft.Json.Serialization;

using Microsoft.IdentityModel.Logging;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;
// Add services to the container.

builder.Services.AddControllersWithViews()
               .AddNewtonsoftJson(options =>
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft
               .Json.ReferenceLoopHandling.Ignore)
               .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
               = new DefaultContractResolver());

builder.Services.AddControllers();

var privateKey = "academic academic academic academic";

builder.Services.AddAuthentication(x =>
{
   x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
   x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //x.RequireHttpsMetadata = false;
    //x.SaveToken = true;
    //x.TokenValidationParameters = new TokenValidationParameters
    //{
    //    ValidateIssuerSigningKey = true,
    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(privateKey))
    //};

    options.SaveToken = true;
    //options.Authority = "https://localhost:7014";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(privateKey)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    options.Audience = "quality";
});


builder.Services.AddSingleton<IJwtAuthenticationManager>(new JwtAuthenticationManager(privateKey));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

app.Run();
