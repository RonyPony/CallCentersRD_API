using CallCentersRD_API.Database;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CallCentersRD_API.Database.Entities.Auth;
using System.Text;
using CallCentersRD_API.Database.Repositories.Constructor;
using CallCentersRD_API.Database.Services.Constructor;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;



// Add services to the container.
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(configuration.GetConnectionString("Postgres")));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddIdentity<User, Role>().AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };
});

builder.Services.AddHealthChecks();
builder.Services.AddAuthorization();
builder.Services.AddScoped<IRepositoryConstructor>(factory => new RepositoryConstructor(factory.GetService<DatabaseContext>()));
builder.Services.AddScoped<IServiceConstructor>(factory => new ServiceConstructor(factory.GetService<IRepositoryConstructor>()));

builder.Services.AddCors();

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

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

// global cors policy
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()
    ); // allow credentials

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
