using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebApiAuthentication.Authentication;
using WebApiAuthentication.DataAccess;
using WebApiAuthentication.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddScoped<IPatientsRepository, PatientsRepository>();

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));

services.AddDbContext<PatientsContext>(optons =>
    optons.UseSqlServer(builder.Configuration.GetConnectionString("ServerDb"))
);

services.AddIdentity<ApplicationUser, IdentityRole>()  
    .AddEntityFrameworkStores<ApplicationDbContext>()  
    .AddDefaultTokenProviders();

services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>  
{  
    options.SaveToken = true;  
    options.RequireHttpsMetadata = false;  
    options.TokenValidationParameters = new TokenValidationParameters()  
    {  
        ValidateIssuer = true,  
        ValidateAudience = true,  
        ValidAudience = builder.Configuration["JWT:ValidAudience"],  
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],  
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))  
    };  
});

#if DEBUG
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = typeof(Program).Namespace, Version = "v1" });

    var scheme = new OpenApiSecurityScheme()
    {
        Name = "Bearer",
        Description = "Please enter you JWT",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    };

    options.AddSecurityDefinition("JWT", scheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "JWT",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});
#endif

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//TODO: Adding JWT token bearer
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();  
app.UseAuthorization();  
  
app.UseEndpoints(endpoints =>  
{  
    endpoints.MapControllers();  
});
app.Run();