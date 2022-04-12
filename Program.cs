
using CallCentersRD_API.Database;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200",
                                              "https://localhost:4200").AllowAnyHeader()
                                                  .AllowAnyMethod();
                      });
});

//builder.Services.AddDbContext<CallCenterDbContext>(opt =>
//{
//    //opt.UseInMemoryDatabase("users");
//    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnections"));
//})

builder.Services.AddDbContextPool<CallCenterDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("someeDB"),
        sqlServerOptionsAction: options => { options.EnableRetryOnFailure(); }
        ));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);
//if (app.Environment.IsDevelopment())

//{
//    app.UseExceptionHandler("/error-development");
//}
//else
//{
//    app.UseExceptionHandler("/error");
//}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
